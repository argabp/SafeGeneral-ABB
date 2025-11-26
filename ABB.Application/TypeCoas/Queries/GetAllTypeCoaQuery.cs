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

            // ✅ Project ke DTO menggunakan AutoMapper
            return await query
                .ProjectTo<TypeCoaDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
