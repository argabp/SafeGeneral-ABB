using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ABB.Application.VoucherKass.Queries;

namespace ABB.Application.PostingVoucherKas.Queries
{
    public class GetAllVoucherKasByFlagQuery : IRequest<List<VoucherKasDto>>
    {
        public bool FlagPosting { get; set; }   // true = sudah posting, false = belum posting
        public string SearchKeyword { get; set; }
         public string DatabaseName { get; set; }   
        public string KodeCabang { get; set; }
        public bool FlagFinal { get; set; }
    }

    public class GetAllVoucherKasByFlagQueryHandler : IRequestHandler<GetAllVoucherKasByFlagQuery, List<VoucherKasDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllVoucherKasByFlagQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<VoucherKasDto>> Handle(GetAllVoucherKasByFlagQuery request, CancellationToken cancellationToken)
        {
            // Ambil data dasar dengan filter FlagPosting
var query =
            from vk in _context.VoucherKas
             join kb in _context.KasBank
                on vk.KodeAkun equals kb.NoPerkiraan into kasBankJoin
            from kb in kasBankJoin.DefaultIfEmpty()

             select new VoucherKasDto
            {
                KodeCabang = vk.KodeCabang,
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


             query = query
                .Where(v => v.FlagPosting == request.FlagPosting)  // filter berdasarkan flag
                .AsQueryable();

            // Jika ada kata kunci pencarian, filter lagi
            if (!string.IsNullOrEmpty(request.SearchKeyword))
            {
                var keyword = request.SearchKeyword.ToLower();
                decimal searchDecimal;
                bool isDecimal = decimal.TryParse(request.SearchKeyword, out searchDecimal);

                query = query.Where(kb =>
                    kb.KodeCabang.ToLower().Contains(keyword) ||
                    kb.JenisVoucher.ToLower().Contains(keyword) ||
                    kb.NoVoucher.ToLower().Contains(keyword) ||
                    kb.KodeAkun.ToLower().Contains(keyword) ||
                    (isDecimal && kb.TotalVoucher.HasValue && kb.TotalVoucher.Value == searchDecimal)
                );
            }

            

            query = query.Where(vk => vk.FlagFinal == request.FlagFinal);

            if (!string.IsNullOrEmpty(request.KodeCabang))
            {
                query = query.Where(vk => vk.KodeCabang == request.KodeCabang);
            }
            // Proyeksikan ke DTO
           var voucherKasList = await query
                .ToListAsync(cancellationToken);

            return voucherKasList;
        }
    }
}
