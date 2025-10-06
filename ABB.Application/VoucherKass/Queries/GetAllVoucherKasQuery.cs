using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ABB.Application.VoucherKass.Queries
{

    public class GetAllVoucherKasQuery : IRequest<List<VoucherKasDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
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
             // Ambil data dasar
            var query = _context.VoucherKas.AsQueryable();

            // JIKA ADA KATA KUNCI, LAKUKAN FILTER
            if (!string.IsNullOrEmpty(request.SearchKeyword))
            {
                var keyword = request.SearchKeyword.ToLower();
                decimal searchDecimal;
                bool isDecimal = decimal.TryParse(request.SearchKeyword, out searchDecimal);

                query = query.Where(kb => 
                    kb.KodeCabang.ToLower().Contains(keyword) ||
                    kb.JenisVoucher.ToLower().Contains(keyword) ||
                    kb.NoVoucher.ToLower().Contains(keyword)||
                    kb.KodeAkun.ToLower().Contains(keyword)||
                   
                    (isDecimal && kb.TotalVoucher.HasValue && kb.TotalVoucher.Value == searchDecimal)
                );
            }

            // Lanjutkan proses seperti biasa dengan data yang sudah difilter
            var VoucherKasList = await query
                .ProjectTo<VoucherKasDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return VoucherKasList;
        }
    }
}