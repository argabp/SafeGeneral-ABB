using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CoaEntity = ABB.Domain.Entities.Coa;

namespace ABB.Application.Coas.Queries
{
    public class GetCoaByIdQuery : IRequest<CoaDto>
    {
        public string Kode { get; set; }
    }

    public class GetCoaByIdQueryHandler : IRequestHandler<GetCoaByIdQuery, CoaDto>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetCoaByIdQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CoaDto> Handle(GetCoaByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Coa
                .FirstOrDefaultAsync(c => c.gl_kode == request.Kode, cancellationToken);
            
            // Konversi dari Entity ke Dto
            var CoaDto = _mapper.Map<CoaDto>(entity);

            return CoaDto;
        }
    }
}