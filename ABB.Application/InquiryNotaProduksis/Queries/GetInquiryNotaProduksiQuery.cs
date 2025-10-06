using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace ABB.Application.InquiryNotaProduksis.Queries
{
    public class InquiryNotaProduksiQuery : IRequest<List<InquiryNotaProduksiDto>>
    {
        public string SearchKeyword { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class InquiryNotaProduksiQueryHandler : IRequestHandler<InquiryNotaProduksiQuery, List<InquiryNotaProduksiDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public InquiryNotaProduksiQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<InquiryNotaProduksiDto>> Handle(InquiryNotaProduksiQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Produksi.AsQueryable();

            // 🔍 Filter keyword
            if (!string.IsNullOrEmpty(request.SearchKeyword))
            {
                var keyword = request.SearchKeyword.ToLower();
                query = query.Where(p =>
                    p.no_nd.ToLower().Contains(keyword) ||
                    p.nm_cust2.ToLower().Contains(keyword)
                );
            }

            // 🔍 Filter tanggal
            if (request.StartDate.HasValue && request.EndDate.HasValue)
            {
                query = query.Where(p => p.date >= request.StartDate && p.date <= request.EndDate);
            }

            // ⏳ Kalau tidak ada filter tanggal → limit 30 data terakhir
            // if (!request.StartDate.HasValue && !request.EndDate.HasValue)
            // {
            //     query = query.OrderByDescending(p => p.date).Take(30);
            // }
            // else
            // {
            //     query = query.OrderByDescending(p => p.date);
            // }

            return await query
                .ProjectTo<InquiryNotaProduksiDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
