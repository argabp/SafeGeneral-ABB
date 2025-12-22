using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.JurnalMemorials104.Queries
{
    public class GetDetailJurnalMemorial104Query : IRequest<List<JurnalMemorial104DetailDto>>
    {
        public string NoVoucher { get; set; }
    }

    public class GetDetailJurnalMemorial104QueryHandler : IRequestHandler<GetDetailJurnalMemorial104Query, List<JurnalMemorial104DetailDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetDetailJurnalMemorial104QueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<JurnalMemorial104DetailDto>> Handle(GetDetailJurnalMemorial104Query request, CancellationToken cancellationToken)
        {
            return await _context.DetailJurnalMemorial104
                .Where(x => x.NoVoucher == request.NoVoucher)
                .OrderBy(x => x.No)
                .ProjectTo<JurnalMemorial104DetailDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}