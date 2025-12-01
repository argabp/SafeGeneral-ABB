using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.TipeAkuns117.Queries
{
    public class GetAllTipeAkun117Query : IRequest<List<TipeAkun117Dto>>
    {
        public string SearchKeyword { get; set; } // ✅ properti pencarian
    }

    public class GetAllTipeAkun117QueryHandler : IRequestHandler<GetAllTipeAkun117Query, List<TipeAkun117Dto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllTipeAkun117QueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TipeAkun117Dto>> Handle(GetAllTipeAkun117Query request, CancellationToken cancellationToken)
        {
            var query = _context.TipeAkun117.AsQueryable();

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
                .ProjectTo<TipeAkun117Dto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
