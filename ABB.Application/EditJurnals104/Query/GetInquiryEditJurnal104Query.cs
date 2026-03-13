using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;

namespace ABB.Application.EditJurnals104.Queries
{
    public class InquiryJurnal104Dto
    {
        public string NoBukti { get; set; }
        public string Lokasi { get; set; }
        public string NamaCabang { get; set; } 
        public DateTime Tanggal { get; set; }
        public string Keterangan { get; set; }
        public decimal TotalNilai { get; set; }
        public string GlTran { get; set; } 
    }

    public class GetInquiryEditJurnal104Query : IRequest<List<InquiryJurnal104Dto>>
    {
        public string DatabaseName { get; set; }
        public DateTime TglAwal { get; set; }
        public DateTime TglAkhir { get; set; }
        public string NoBukti { get; set; }
    }

    public class GetInquiryEditJurnal104QueryHandler : IRequestHandler<GetInquiryEditJurnal104Query, List<InquiryJurnal104Dto>>
    {
        private readonly IDbContextPstNota _context;

        public GetInquiryEditJurnal104QueryHandler(IDbContextPstNota context)
        {
            _context = context;
        }

       public async Task<List<InquiryJurnal104Dto>> Handle(GetInquiryEditJurnal104Query request, CancellationToken cancellationToken)
        {
            var batasAwal = request.TglAwal.Date;
            var batasAkhir = request.TglAkhir.Date.AddDays(1);

            // 1. FILTER DASAR JURNAL
            var queryJurnal = _context.Set<Jurnal62>()
                .AsNoTracking()
                .Where(x => x.FlagClosed == false &&
                            x.GlTanggal >= batasAwal && 
                            x.GlTanggal < batasAkhir);

            if (!string.IsNullOrWhiteSpace(request.NoBukti))
            {
                var noBuktiClean = request.NoBukti.Trim();
                queryJurnal = queryJurnal.Where(x => x.GlBukti == noBuktiClean);
            }

            // 2. GROUPING DATA JURNAL
            var groupedJurnal = queryJurnal
                .GroupBy(x => x.GlBukti)
                .Select(g => new 
                {
                    NoBukti = g.Key,
                    Lokasi = g.Max(x => x.GlLok),
                    Tanggal = g.Max(x => x.GlTanggal),
                    Keterangan = g.Max(x => x.GlKet),
                    GlTran = g.Max(x => x.GlTran), 
                    TotalNilaiDouble = g.Where(x => x.GlDk == "D").Sum(x => x.GlNilaiIdr) 
                });

            // 3. LEFT JOIN MANUAL (Cocokkan dengan akhiran, misal "JK50" berakhiran "50")
            var joinedQuery = 
                from j in groupedJurnal
                from cb in _context.Set<Cabang>() 
                    .Where(c => c.kd_cb != null && j.Lokasi != null && c.kd_cb.Trim().EndsWith(j.Lokasi.Trim()))
                    .DefaultIfEmpty()
                select new 
                {
                    NoBukti = j.NoBukti,
                    KodeLokasi = j.Lokasi.Trim(), 
                    NamaCabang = cb != null ? cb.nm_cb.Trim() : "", 
                    Tanggal = j.Tanggal,
                    Keterangan = j.Keterangan,
                    GlTran = j.GlTran,
                    TotalNilaiDouble = j.TotalNilaiDouble
                };

            // 4. TARIK DATA KE MEMORI
            var rawResult = await joinedQuery.ToListAsync(cancellationToken);

            // 5. MAPPING FINAL KE DTO
            var result = rawResult
                .Select(x => new InquiryJurnal104Dto
                {
                    NoBukti = x.NoBukti,
                    Lokasi = x.KodeLokasi, 
                    
                    // --- GABUNGKAN NAMA CABANG & LOKASI DI SINI ---
                    // Biar kalau kosong dia cuma nampilin "50", tapi kalau ada jadi "50 - JAKARTA"
                    NamaCabang = !string.IsNullOrWhiteSpace(x.NamaCabang) 
                                 ? $"{x.KodeLokasi} - {x.NamaCabang}" 
                                 : x.KodeLokasi, 
                    
                    Tanggal = x.Tanggal ?? DateTime.Now,
                    Keterangan = x.Keterangan,
                    GlTran = x.GlTran, 
                    TotalNilai = Convert.ToDecimal(x.TotalNilaiDouble ?? 0)
                })
                .OrderByDescending(x => x.Tanggal)
                .ToList();

            return result;
        }
    }
}