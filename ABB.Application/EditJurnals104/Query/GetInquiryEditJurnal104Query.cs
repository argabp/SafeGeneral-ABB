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
        public DateTime Tanggal { get; set; }
        public string Keterangan { get; set; }
        public decimal TotalNilai { get; set; }
        
        // --- 1. TAMBAH KERANJANG GLTRAN DI SINI ---
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

            var query = _context.Set<Jurnal62>()
                .AsNoTracking()
                .Where(x => x.FlagClosed == false &&
                            x.GlTanggal >= batasAwal && 
                        x.GlTanggal < batasAkhir);

            // Jika Nomor Bukti diisi, tambahkan filter
            if (!string.IsNullOrWhiteSpace(request.NoBukti))
            {
                var noBuktiClean = request.NoBukti.Trim();
                query = query.Where(x => x.GlBukti == noBuktiClean);
            }

            // STEP 1: Tarik data dari SQL Server
            var rawResult = await query
                .GroupBy(x => x.GlBukti)
                .Select(g => new 
                {
                    NoBukti = g.Key,
                    Lokasi = g.Max(x => x.GlLok),
                    Tanggal = g.Max(x => x.GlTanggal),
                    Keterangan = g.Max(x => x.GlKet),
                    
                    // --- 2. TARIK GL_TRAN DARI DATABASE ---
                    GlTran = g.Max(x => x.GlTran), 
                    
                    TotalNilaiDouble = g.Where(x => x.GlDk == "D").Sum(x => x.GlNilaiIdr) 
                })
                .ToListAsync(cancellationToken);

            // STEP 2: Mapping
            var result = rawResult
                .Select(x => new InquiryJurnal104Dto
                {
                    NoBukti = x.NoBukti,
                    Lokasi = x.Lokasi,
                    Tanggal = x.Tanggal ?? DateTime.Now,
                    Keterangan = x.Keterangan,
                    
                    // --- 3. MASUKKAN KE KERANJANG DTO ---
                    GlTran = x.GlTran, 
                    
                    TotalNilai = Convert.ToDecimal(x.TotalNilaiDouble ?? 0)
                })
                .OrderByDescending(x => x.Tanggal)
                .ToList();

            return result;
        }
    }
}