using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.TypeCoas.Queries
{
    public class GetTypeCoaByIdQuery : IRequest<TypeCoaDto>
    {
        public string Type { get; set; }
    }

    public class GetTypeCoaByIdQueryHandler : IRequestHandler<GetTypeCoaByIdQuery, TypeCoaDto>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetTypeCoaByIdQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TypeCoaDto> Handle(GetTypeCoaByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.TypeCoa
                .FirstOrDefaultAsync(x => x.Type == request.Type, cancellationToken);

            if (entity == null) return null;

            return _mapper.Map<TypeCoaDto>(entity);
        }
    }
}