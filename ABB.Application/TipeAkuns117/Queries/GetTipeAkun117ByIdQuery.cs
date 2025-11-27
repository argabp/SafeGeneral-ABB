using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.TipeAkuns117.Queries
{
    public class GetTipeAkun117ByIdQuery : IRequest<TipeAkun117Dto>
    {
        public string Kode { get; set; }
    }

    public class GetTipeAkun117ByIdQueryHandler : IRequestHandler<GetTipeAkun117ByIdQuery, TipeAkun117Dto>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetTipeAkun117ByIdQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TipeAkun117Dto> Handle(GetTipeAkun117ByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.TipeAkun117
                .FirstOrDefaultAsync(x => x.Kode == request.Kode, cancellationToken);

            if (entity == null) return null;

            return _mapper.Map<TipeAkun117Dto>(entity);
        }
    }
}