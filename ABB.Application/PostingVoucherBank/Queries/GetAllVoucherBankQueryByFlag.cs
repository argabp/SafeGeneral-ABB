using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ABB.Application.VoucherBanks.Queries;

namespace ABB.Application.PostingVoucherBank.Queries
{
    public class GetAllVoucherBankByFlagQuery : IRequest<List<VoucherBankDto>>
    {
        public bool FlagPosting { get; set; }
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
        public bool FlagFinal { get; set; }
    }

    public class GetAllVoucherBankByFlagQueryHandler 
        : IRequestHandler<GetAllVoucherBankByFlagQuery, List<VoucherBankDto>>
    {
        private readonly IDbContextPstNota _context;

        public GetAllVoucherBankByFlagQueryHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<List<VoucherBankDto>> Handle(
            GetAllVoucherBankByFlagQuery request, 
            CancellationToken cancellationToken)
        {
            var query =
                from vb in _context.VoucherBank

                join kb in _context.KasBank
                    on new { vb.KodeAkun, vb.KodeCabang }
                    equals new { KodeAkun = kb.NoPerkiraan, kb.KodeCabang }
                    into kasBankJoin
                from kb in kasBankJoin.DefaultIfEmpty()

                where vb.FlagPosting == request.FlagPosting
                   && vb.FlagFinal == request.FlagFinal
                   && _context.EntriPembayaranBank
                        .Any(epb => epb.NoVoucher == vb.NoVoucher)

                select new VoucherBankDto
                {
                    Id = vb.Id,
                    KodeCabang = vb.KodeCabang,
                    JenisVoucher = vb.JenisVoucher,
                    NoVoucher = vb.NoVoucher,
                    KodeAkun = vb.KodeAkun,
                    NoVoucherSementara = vb.NoVoucherSementara,
                    FlagSementara = vb.FlagSementara ?? false,
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

            // Filter Kode Cabang
            if (!string.IsNullOrEmpty(request.KodeCabang))
            {
                query = query.Where(v => v.KodeCabang == request.KodeCabang);
            }

            // Filter Search
            if (!string.IsNullOrEmpty(request.SearchKeyword))
            {
                var keyword = request.SearchKeyword.ToLower();
                decimal searchDecimal;
                bool isDecimal = decimal.TryParse(request.SearchKeyword, out searchDecimal);

                query = query.Where(kb =>
                    (kb.KodeCabang ?? "").ToLower().Contains(keyword) ||
                    (kb.JenisVoucher ?? "").ToLower().Contains(keyword) ||
                    (kb.NoVoucher ?? "").ToLower().Contains(keyword) ||
                    (kb.KodeAkun ?? "").ToLower().Contains(keyword) ||
                    (kb.NamaBank ?? "").ToLower().Contains(keyword) ||
                    (isDecimal && kb.TotalVoucher.HasValue && kb.TotalVoucher.Value == searchDecimal)
                );
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}