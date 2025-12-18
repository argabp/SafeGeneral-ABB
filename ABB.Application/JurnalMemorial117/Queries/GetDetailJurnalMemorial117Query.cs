using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.JurnalMemorial117.Queries
{
    public class GetDetailJurnalMemorial117Query : IRequest<List<JurnalMemorial117DetailDto>>
    {
        public string NoVoucher { get; set; }
    }

    public class GetDetailJurnalMemorial117QueryHandler : IRequestHandler<GetDetailJurnalMemorial117Query, List<JurnalMemorial117DetailDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetDetailJurnalMemorial117QueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<JurnalMemorial117DetailDto>> Handle(GetDetailJurnalMemorial117Query request, CancellationToken cancellationToken)
        {
            return await _context.JurnalMemorial117Detail
                .Where(x => x.NoVoucher == request.NoVoucher)
                .OrderBy(x => x.No)
                .ProjectTo<JurnalMemorial117DetailDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}