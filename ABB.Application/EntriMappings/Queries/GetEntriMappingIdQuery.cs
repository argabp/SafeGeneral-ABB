using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EntriMappingEntity = ABB.Domain.Entities.EntriMapping;

namespace ABB.Application.EntriMappings.Queries
{
    public class GetEntriMappingByIdQuery : IRequest<EntriMappingDto>
    {
        public string gl_akun104 { get; set; }
        public string gl_akun117 { get; set; }
    }

    public class GetEntriMappingByIdQueryHandler : IRequestHandler<GetEntriMappingByIdQuery, EntriMappingDto>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetEntriMappingByIdQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EntriMappingDto> Handle(GetEntriMappingByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.EntriMapping
                .FirstOrDefaultAsync(c => c.gl_akun104 == request.gl_akun104, cancellationToken);
            
            // Konversi dari Entity ke Dto
            var EntriMappingDto = _mapper.Map<EntriMappingDto>(entity);

            return EntriMappingDto;
        }
    }
}