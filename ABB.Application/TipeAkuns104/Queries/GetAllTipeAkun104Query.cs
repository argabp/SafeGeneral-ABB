using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.TipeAkuns104.Queries
{
    public class GetAllTipeAkun104Query : IRequest<List<TipeAkun104Dto>>
    {
        public string SearchKeyword { get; set; } // ✅ properti pencarian
    }

    public class GetAllTipeAkun104QueryHandler : IRequestHandler<GetAllTipeAkun104Query, List<TipeAkun104Dto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllTipeAkun104QueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TipeAkun104Dto>> Handle(GetAllTipeAkun104Query request, CancellationToken cancellationToken)
        {
            var query = _context.TipeAkun104.AsQueryable();

            // ✅ Tambahkan filter pencarian
            if (!string.IsNullOrWhiteSpace(request.SearchKeyword))
            {
                string keyword = request.SearchKeyword.ToLower();
                query = query.Where(x =>
                    x.Kode.ToLower().Contains(keyword) ||
                    x.NamaTipe.ToLower().Contains(keyword) ||
                    x.Pos.ToLower().Contains(keyword));
            }

            // ✅ Project ke DTO menggunakan AutoMapper
            return await query
                .ProjectTo<TipeAkun104Dto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
