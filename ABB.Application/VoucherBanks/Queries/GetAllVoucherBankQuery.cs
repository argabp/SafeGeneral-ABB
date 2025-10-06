using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using VoucherBankEntity = ABB.Domain.Entities.VoucherBank;

namespace ABB.Application.VoucherBanks.Queries
{
    public class GetAllVoucherBankQuery : IRequest<List<VoucherBankDto>>
    {
         public string SearchKeyword { get; set; }
    }

    public class GetAllVoucherBankQueryHandler : IRequestHandler<GetAllVoucherBankQuery, List<VoucherBankDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllVoucherBankQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<VoucherBankDto>> Handle(GetAllVoucherBankQuery request, CancellationToken cancellationToken)
        {
            var query = _context.VoucherBank.AsQueryable();

            // JIKA ADA KATA KUNCI, LAKUKAN FILTER
            if (!string.IsNullOrEmpty(request.SearchKeyword))
            {
                var keyword = request.SearchKeyword.ToLower();
                
                // --- FIX: Deklarasikan isDecimal dan searchDecimal di sini ---
                decimal searchDecimal;
                bool isDecimal = decimal.TryParse(request.SearchKeyword, out searchDecimal);
                // -------------------------------------------------------------

                query = query.Where(kb =>
                    kb.KodeCabang.ToLower().Contains(keyword) ||
                    kb.JenisVoucher.ToLower().Contains(keyword) ||
                    kb.NoVoucher.ToLower().Contains(keyword) ||
                    kb.KodeAkun.ToLower().Contains(keyword) ||
                    kb.DiterimaDari.ToLower().Contains(keyword) ||
                    // kb.TanggalVoucher.Value.ToString().Contains(keyword) || // Ini tidak efisien dan bisa error
                    kb.KeteranganVoucher.ToLower().Contains(keyword) ||
                    kb.KodeBank.ToLower().Contains(keyword) ||
                    (isDecimal && kb.TotalVoucher.HasValue && kb.TotalVoucher.Value == searchDecimal)
                );
            }

            var list = await query
                .ProjectTo<VoucherBankDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return list;
        }
    }
}