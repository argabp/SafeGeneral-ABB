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
    public class GetAllJurnalMemorial117Query : IRequest<List<JurnalMemorial117Dto>>
    {
        public string SearchKeyword { get; set; }
        public string KodeCabang { get; set; }
    }

    public class GetAllJurnalMemorial117QueryHandler : IRequestHandler<GetAllJurnalMemorial117Query, List<JurnalMemorial117Dto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllJurnalMemorial117QueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<JurnalMemorial117Dto>> Handle(GetAllJurnalMemorial117Query request, CancellationToken cancellationToken)
        {
            var query = _context.JurnalMemorial117
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
                .ProjectTo<JurnalMemorial117Dto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}