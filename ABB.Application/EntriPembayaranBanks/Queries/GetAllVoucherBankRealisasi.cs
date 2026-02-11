using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ABB.Application.VoucherBanks.Queries;

namespace ABB.Application.EntriPembayaranBanks.Queries
{
    public class GetAllVoucherBankRealisasiQuery : IRequest<List<VoucherBankDto>>
    {
        public string SearchKeyword { get; set; }
         public string KodeCabang { get; set; }
         public bool FlagFinal { get; set; }
    }

    public class GetAllVoucherBankRealisasiQueryHandler : IRequestHandler<GetAllVoucherBankRealisasiQuery, List<VoucherBankDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllVoucherBankRealisasiQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<VoucherBankDto>> Handle(GetAllVoucherBankRealisasiQuery request, CancellationToken cancellationToken)
        {
            // JOIN VoucherBank dengan Cabang
            var query =
                from vb in _context.VoucherBank
                join cb in _context.Cabang
                    on vb.KodeCabang equals cb.kd_cb into cabangJoin
                from cb in cabangJoin.DefaultIfEmpty()

                 join kb in _context.KasBank
                on vb.KodeAkun equals kb.NoPerkiraan into kasBankJoin
                from kb in kasBankJoin.DefaultIfEmpty()
                select new VoucherBankDto
                {
                    KodeCabang = vb.KodeCabang,
                    NamaCabang = cb != null ? cb.nm_cb : null,
                    JenisVoucher = vb.JenisVoucher,
                    NoVoucher = vb.NoVoucher,
                    KodeAkun = vb.KodeAkun,
                    DebetKredit = vb.DebetKredit,
                    DiterimaDari = vb.DiterimaDari,
                    TanggalVoucher = vb.TanggalVoucher,
                    KodeBank = vb.KodeBank,
                    TotalVoucher = vb.TotalVoucher,
                    TotalDalamRupiah = vb.TotalDalamRupiah,
                    KeteranganVoucher = vb.KeteranganVoucher,
                    FlagPosting = vb.FlagPosting ?? false,
                    TanggalInput = vb.TanggalInput,
                    TanggalUpdate = vb.TanggalUpdate,
                    KodeUserInput = vb.KodeUserInput,
                    KodeUserUpdate = vb.KodeUserUpdate,
                    JenisPembayaran = vb.JenisPembayaran,
                    FlagFinal = vb.FlagFinal ?? false,
                    NamaBank = kb != null ? kb.Keterangan : null
                };

            query = query.Where(vb => vb.FlagFinal == request.FlagFinal);

            query = query.Where(vb => !string.IsNullOrWhiteSpace(vb.NoVoucher));

            if (!string.IsNullOrEmpty(request.KodeCabang))
            {
                query = query.Where(vb => vb.KodeCabang == request.KodeCabang);
            }
            // FILTER PENCARIAN
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
                    kb.DiterimaDari.ToLower().Contains(keyword) ||
                    kb.KeteranganVoucher.ToLower().Contains(keyword) ||
                    kb.KodeBank.ToLower().Contains(keyword) ||
                    (isDecimal && kb.TotalVoucher.HasValue && kb.TotalVoucher.Value == searchDecimal)
                );
            }

            // EXECUTE QUERY
            var list = await query.ToListAsync(cancellationToken);

            return list;
        }
    }
}
