using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPeriodes.Queries
{
    public class GetAllPeriodeQuery : IRequest<List<EntriPeriodeDto>>
    {
        public string SearchKeyword { get; set; } // Misal cari tahun
    }

    public class GetAllPeriodeQueryHandler : IRequestHandler<GetAllPeriodeQuery, List<EntriPeriodeDto>>
    {
        private readonly IDbContextPstNota _context; // Sesuaikan nama Context Anda
        private readonly IMapper _mapper;

        public GetAllPeriodeQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<EntriPeriodeDto>> Handle(GetAllPeriodeQuery request, CancellationToken cancellationToken)
        {
            var query = _context.EntriPeriode.AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchKeyword))
            {
                // Contoh filter: jika user mengetik tahun (misal "2024")
                if (decimal.TryParse(request.SearchKeyword, out decimal tahun))
                {
                    query = query.Where(x => x.ThnPrd == tahun);
                }
            }

            return await query
                .OrderByDescending(x => x.ThnPrd)
                .ThenByDescending(x => x.BlnPrd)
                .ProjectTo<EntriPeriodeDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}