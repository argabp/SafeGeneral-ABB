using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ABB.Application.VoucherKass.Queries;

namespace ABB.Application.EntriPembayaranKass.Queries
{
    public class GetAllVoucherKasRealisasiQuery : IRequest<List<VoucherKasDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
        public bool FlagFinal { get; set; }
    }

    public class GetAllVoucherKasRealisasiQueryHandler : IRequestHandler<GetAllVoucherKasRealisasiQuery, List<VoucherKasDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllVoucherKasRealisasiQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<VoucherKasDto>> Handle(GetAllVoucherKasRealisasiQuery request, CancellationToken cancellationToken)
        {
            // Ambil data dasar + join nama cabang
           var query =
            from vk in _context.VoucherKas
            join cb in _context.Cabang
                on vk.KodeCabang equals cb.kd_cb into cabangJoin
            from cb in cabangJoin.DefaultIfEmpty()

            // JOIN ke KasBank berdasarkan NoPerkiraan
                join kb in _context.KasBank
                        on new { 
                                KodeAkun = vk.KodeAkun.Trim(),
                                KodeCabang = vk.KodeCabang.Trim(),
                                KodeKas = vk.KodeKas.Trim() 
                                }
                        equals new 
                        { 
                            KodeAkun = kb.NoPerkiraan.Trim(),
                            KodeCabang = kb.KodeCabang.Trim(),
                            KodeKas = kb.Kode.Trim()
                        }
                        into kasBankJoin
                from kb in kasBankJoin.DefaultIfEmpty()

            select new VoucherKasDto
            {
                KodeCabang = vk.KodeCabang,
                NamaCabang = cb != null ? cb.nm_cb : null,

                JenisVoucher = vk.JenisVoucher,
                DebetKredit = vk.DebetKredit,
                NoVoucher = vk.NoVoucher,

                KodeAkun = vk.KodeAkun,
                NamaKas = kb != null ? kb.Keterangan : null, // 👈 hasil join

                DibayarKepada = vk.DibayarKepada,
                KodeKas = vk.KodeKas,
                TanggalVoucher = vk.TanggalVoucher,
                KodeMataUang = vk.KodeMataUang,
                TotalVoucher = vk.TotalVoucher,
                TotalDalamRupiah = vk.TotalDalamRupiah,
                KeteranganVoucher = vk.KeteranganVoucher,

                FlagPosting = vk.FlagPosting ?? false,
                FlagFinal = vk.FlagFinal ?? false,

                TanggalInput = vk.TanggalInput,
                TanggalUpdate = vk.TanggalUpdate,
                KodeUserInput = vk.KodeUserInput,
                KodeUserUpdate = vk.KodeUserUpdate,

                JenisPembayaran = vk.JenisPembayaran
            };

             query = query.Where(vk => !string.IsNullOrWhiteSpace(vk.NoVoucher));
             query = query.Where(vk => vk.FlagFinal == request.FlagFinal);

            if (!string.IsNullOrEmpty(request.KodeCabang))
            {
                query = query.Where(vk => vk.KodeCabang == request.KodeCabang);
            }
            // FILTER pencarian
            if (!string.IsNullOrEmpty(request.SearchKeyword))
            {
                var keyword = request.SearchKeyword.ToLower();
                decimal searchDecimal;
                bool isDecimal = decimal.TryParse(request.SearchKeyword, out searchDecimal);

                query = query.Where(kb =>
                    kb.KodeCabang.ToLower().Contains(keyword) ||
                    (kb.NamaCabang ?? "").ToLower().Contains(keyword) ||
                    kb.JenisVoucher.ToLower().Contains(keyword) ||
                    kb.NoVoucher.ToLower().Contains(keyword) ||
                    kb.KodeAkun.ToLower().Contains(keyword) ||
                    (isDecimal && kb.TotalVoucher.HasValue && kb.TotalVoucher.Value == searchDecimal)
                );
            }

            // Ambil hasil akhir
            var VoucherKasList = await query.ToListAsync(cancellationToken);

            return VoucherKasList;
        }
    }
}
