using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPenyelesaianPiutangs.Queries
{
    public class GetHeaderPenyelesaianUtangByIdQuery : IRequest<HeaderPenyelesaianUtangDto>
    {
        public string KodeCabang { get; set; }
        public string NomorBukti { get; set; }
    }

    public class GetHeaderPenyelesaianUtangByIdQueryHandler : IRequestHandler<GetHeaderPenyelesaianUtangByIdQuery, HeaderPenyelesaianUtangDto>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetHeaderPenyelesaianUtangByIdQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<HeaderPenyelesaianUtangDto> Handle(GetHeaderPenyelesaianUtangByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.HeaderPenyelesaianUtang
                .FirstOrDefaultAsync(h => h.KodeCabang == request.KodeCabang && h.NomorBukti == request.NomorBukti, cancellationToken);
            
            return _mapper.Map<HeaderPenyelesaianUtangDto>(entity);
        }
    }
}