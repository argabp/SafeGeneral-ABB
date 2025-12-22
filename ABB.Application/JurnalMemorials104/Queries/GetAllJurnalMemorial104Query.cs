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
    public class GetAllJurnalMemorial104Query : IRequest<List<JurnalMemorial104Dto>>
    {
        public string SearchKeyword { get; set; }
        public string KodeCabang { get; set; }
    }

    public class GetAllJurnalMemorial104QueryHandler : IRequestHandler<GetAllJurnalMemorial104Query, List<JurnalMemorial104Dto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllJurnalMemorial104QueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<JurnalMemorial104Dto>> Handle(GetAllJurnalMemorial104Query request, CancellationToken cancellationToken)
        {
            var query = _context.JurnalMemorial104
                .Where(x => x.KodeCabang == request.KodeCabang)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchKeyword))
            {
                var search = request.SearchKeyword.ToLower();
                query = query.Where(x => 
                    x.NoVoucher.ToLower().Contains(search) || 
                    x.Keterangan.ToLower().Contains(search));
            }

            return await query
                .OrderByDescending(x => x.Tanggal)
                .ProjectTo<JurnalMemorial104Dto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}