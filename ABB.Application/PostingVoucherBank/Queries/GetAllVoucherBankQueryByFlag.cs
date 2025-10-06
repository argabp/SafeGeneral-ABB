using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ABB.Application.VoucherBanks.Queries;

namespace ABB.Application.PostingVoucherBank.Queries
{
    public class GetAllVoucherBankByFlagQuery : IRequest<List<VoucherBankDto>>
    {
        public bool FlagPosting { get; set; }   // true = sudah posting, false = belum posting
        public string SearchKeyword { get; set; }
         public string DatabaseName { get; set; }   
    }

    public class GetAllVoucherBankByFlagQueryHandler : IRequestHandler<GetAllVoucherBankByFlagQuery, List<VoucherBankDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllVoucherBankByFlagQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<VoucherBankDto>> Handle(GetAllVoucherBankByFlagQuery request, CancellationToken cancellationToken)
        {
            // Ambil data dasar dengan filter FlagPosting
            var query = _context.VoucherBank
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

            // Proyeksikan ke DTO
            var voucherBankList = await query
                .ProjectTo<VoucherBankDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return voucherBankList;
        }
    }
}
