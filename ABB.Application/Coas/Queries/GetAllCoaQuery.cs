using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.Coas.Queries
{
    public class GetAllCoaQuery : IRequest<List<CoaDto>> { }

    public class GetAllCoaQueryHandler : IRequestHandler<GetAllCoaQuery, List<CoaDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllCoaQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CoaDto>> Handle(GetAllCoaQuery request, CancellationToken cancellationToken)
        {
            return await _context.Coa
                .ProjectTo<CoaDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}