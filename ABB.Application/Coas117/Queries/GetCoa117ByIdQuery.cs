using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Coa117Entity = ABB.Domain.Entities.Coa117;

namespace ABB.Application.Coas117.Queries
{
    public class GetCoa117ByIdQuery : IRequest<Coa117Dto>
    {
        public string Kode { get; set; }
    }

    public class GetCoa117ByIdQueryHandler : IRequestHandler<GetCoa117ByIdQuery, Coa117Dto>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetCoa117ByIdQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Coa117Dto> Handle(GetCoa117ByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Coa117
                .FirstOrDefaultAsync(c => c.gl_kode == request.Kode, cancellationToken);
            
            // Konversi dari Entity ke Dto
            var Coa117Dto = _mapper.Map<Coa117Dto>(entity);

            return Coa117Dto;
        }
    }
}