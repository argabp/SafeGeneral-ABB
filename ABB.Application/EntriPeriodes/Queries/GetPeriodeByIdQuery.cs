using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPeriodes.Queries
{
    public class GetPeriodeByIdQuery : IRequest<EntriPeriodeDto>
    {
        public decimal ThnPrd { get; set; }
        public short BlnPrd { get; set; }
    }

    public class GetPeriodeByIdQueryHandler : IRequestHandler<GetPeriodeByIdQuery, EntriPeriodeDto>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetPeriodeByIdQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EntriPeriodeDto> Handle(GetPeriodeByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.EntriPeriode
                .FirstOrDefaultAsync(x => x.ThnPrd == request.ThnPrd && x.BlnPrd == request.BlnPrd, cancellationToken);

            if (entity == null) return null;

            return _mapper.Map<EntriPeriodeDto>(entity);
        }
    }
}