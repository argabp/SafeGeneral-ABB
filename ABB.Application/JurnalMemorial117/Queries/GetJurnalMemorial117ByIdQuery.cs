using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.JurnalMemorial117.Queries
{
    public class GetJurnalMemorial117ByIdQuery : IRequest<JurnalMemorial117Dto>
    {
        public string KodeCabang { get; set; }
        public string NoVoucher { get; set; }
    }

    public class GetJurnalMemorial117ByIdQueryHandler : IRequestHandler<GetJurnalMemorial117ByIdQuery, JurnalMemorial117Dto>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetJurnalMemorial117ByIdQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<JurnalMemorial117Dto> Handle(GetJurnalMemorial117ByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.JurnalMemorial117
                .FirstOrDefaultAsync(x => x.KodeCabang == request.KodeCabang && x.NoVoucher == request.NoVoucher, cancellationToken);

            if (entity == null) return null;

            return _mapper.Map<JurnalMemorial117Dto>(entity);
        }
    }
}