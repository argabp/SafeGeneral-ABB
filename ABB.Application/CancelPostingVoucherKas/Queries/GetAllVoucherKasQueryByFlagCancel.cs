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

namespace ABB.Application.CancelPostingVoucherKas.Queries
{
    public class GetAllVoucherKasByFlagCancelQuery : IRequest<List<VoucherKasDto>>
    {
        public bool FlagPosting { get; set; }   // true = sudah posting, false = belum posting
        public string SearchKeyword { get; set; }
         public string DatabaseName { get; set; }   
    }

    public class GetAllVoucherKasByFlagCancelQueryHandler : IRequestHandler<GetAllVoucherKasByFlagCancelQuery, List<VoucherKasDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllVoucherKasByFlagCancelQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<VoucherKasDto>> Handle(GetAllVoucherKasByFlagCancelQuery request, CancellationToken cancellationToken)
        {
            // Ambil data dasar dengan filter FlagPosting
            var query = _context.VoucherKas
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
            var voucherKasList = await query
                .ProjectTo<VoucherKasDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return voucherKasList;
        }
    }
}
