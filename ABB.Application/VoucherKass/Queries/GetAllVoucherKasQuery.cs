using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.VoucherKass.Queries
{
    public class GetAllVoucherKasQuery : IRequest<List<VoucherKasDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
        public bool? FlagFinal { get; set; }
    }

    public class GetAllVoucherKasQueryHandler : IRequestHandler<GetAllVoucherKasQuery, List<VoucherKasDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllVoucherKasQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<VoucherKasDto>> Handle(GetAllVoucherKasQuery request, CancellationToken cancellationToken)
        {
            // Ambil data dasar + join nama cabang
           var query =
            from vk in _context.VoucherKas
            join cb in _context.Cabang
                on vk.KodeCabang equals cb.kd_cb into cabangJoin
            from cb in cabangJoin.DefaultIfEmpty()

            // JOIN ke KasBank berdasarkan NoPerkiraan
            join kb in _context.KasBank
                    on new { vk.KodeAkun, vk.KodeCabang } 
                    equals new { KodeAkun = kb.NoPerkiraan, kb.KodeCabang } 
                    into kasBankJoin
                from kb in kasBankJoin.DefaultIfEmpty()

            select new VoucherKasDto
            {
                KodeCabang = vk.KodeCabang,
                NamaCabang = cb != null ? cb.nm_cb : null,

                JenisVoucher = vk.JenisVoucher,
                Id = vk.Id,
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
                FlagSementara = vk.FlagSementara ?? false,
                NoVoucherSementara = vk.NoVoucherSementara,

                TanggalInput = vk.TanggalInput,
                TanggalUpdate = vk.TanggalUpdate,
                KodeUserInput = vk.KodeUserInput,
                KodeUserUpdate = vk.KodeUserUpdate,

                JenisPembayaran = vk.JenisPembayaran
            };
            //  query = query.Where(vk => vk.FlagFinal == request.FlagFinal);
            if (request.FlagFinal.HasValue)
            {
                query = query.Where(x => x.FlagFinal == request.FlagFinal.Value);
            }
            
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

                // [PERBAIKAN 3] Handle NULL pada NoVoucher agar tidak error
                query = query.Where(x =>
                    (x.KodeCabang ?? "").ToLower().Contains(keyword) ||
                    (x.NamaCabang ?? "").ToLower().Contains(keyword) ||
                    (x.JenisVoucher ?? "").ToLower().Contains(keyword) ||
                    (x.NoVoucher ?? "").ToLower().Contains(keyword) || // <-- AMAN DARI NULL
                    (x.NoVoucherSementara ?? "").ToLower().Contains(keyword) || // <-- Cari juga No Smt
                    (x.KodeAkun ?? "").ToLower().Contains(keyword) ||
                    (x.KeteranganVoucher ?? "").ToLower().Contains(keyword) ||
                    (isDecimal && x.TotalVoucher.HasValue && x.TotalVoucher.Value == searchDecimal)
                );
            }

            // Ambil hasil akhir
            var VoucherKasList = await query.ToListAsync(cancellationToken);

            return VoucherKasList;
        }
    }
}
