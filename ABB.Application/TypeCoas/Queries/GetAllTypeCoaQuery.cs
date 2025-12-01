using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.TypeCoas.Queries
{
    public class GetAllTypeCoaQuery : IRequest<List<TypeCoaDto>>
    {
        public string SearchKeyword { get; set; }
    }

    public class GetAllTypeCoaQueryHandler : IRequestHandler<GetAllTypeCoaQuery, List<TypeCoaDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllTypeCoaQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TypeCoaDto>> Handle(GetAllTypeCoaQuery request, CancellationToken cancellationToken)
        {
            var query = _context.TypeCoa.AsQueryable();

             // ✅ Tambahkan filter pencarian
            if (!string.IsNullOrWhiteSpace(request.SearchKeyword))
            {
                string keyword = request.SearchKeyword.ToLower();
                query = query.Where(x =>
                    x.Type.ToLower().Contains(keyword) ||
                    x.Nama.ToLower().Contains(keyword) ||
                    x.Pos.ToLower().Contains(keyword));
            }

            // ✅ Project ke DTO menggunakan AutoMapper
            return await query
                .ProjectTo<TypeCoaDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
