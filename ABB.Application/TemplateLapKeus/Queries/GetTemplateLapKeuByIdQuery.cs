using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.TemplateLapKeus.Queries
{
    public class GetTemplateLapKeuByIdQuery : IRequest<TemplateLapKeuDto>
    {
        public long Id { get; set; }
    }

    public class GetTemplateLapKeuByIdQueryHandler : IRequestHandler<GetTemplateLapKeuByIdQuery, TemplateLapKeuDto>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetTemplateLapKeuByIdQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TemplateLapKeuDto> Handle(GetTemplateLapKeuByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.TemplateLapKeu.FindAsync(request.Id);
            return _mapper.Map<TemplateLapKeuDto>(entity);
        }
    }
}