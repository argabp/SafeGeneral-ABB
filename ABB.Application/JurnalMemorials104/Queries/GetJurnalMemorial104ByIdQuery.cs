using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.JurnalMemorials104.Queries
{
    public class GetJurnalMemorial104ByIdQuery : IRequest<JurnalMemorial104Dto>
    {
        public string KodeCabang { get; set; }
        public string NoVoucher { get; set; }
    }

    public class GetJurnalMemorial104ByIdQueryHandler : IRequestHandler<GetJurnalMemorial104ByIdQuery, JurnalMemorial104Dto>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetJurnalMemorial104ByIdQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<JurnalMemorial104Dto> Handle(GetJurnalMemorial104ByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.JurnalMemorial104
                .FirstOrDefaultAsync(x => x.KodeCabang == request.KodeCabang && x.NoVoucher == request.NoVoucher, cancellationToken);

            if (entity == null) return null;

            return _mapper.Map<JurnalMemorial104Dto>(entity);
        }
    }
}