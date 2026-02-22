using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.Coas.Queries
{
    public class GetAllCoaQuery : IRequest<List<CoaDto>>
    {
        public string SearchKeyword { get; set; }
        public string KodeCabang { get; set; }
    }

    public class GetAllCoaQueryHandler : IRequestHandler<GetAllCoaQuery, List<CoaDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllCoaQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CoaDto>> Handle(GetAllCoaQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Coa.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.KodeCabang))
            {
                string dept = request.KodeCabang.Trim();

                var deptQuery = _context.Coa
                    .Where(x => x.gl_dept.Trim() == dept);

                if (dept == "10")
                {
                    var rangeQuery = _context.Coa
                        .Where(x => 
                            x.gl_kode.Trim().CompareTo("16011100") >= 0 &&
                            x.gl_kode.Trim().CompareTo("16070100") <= 0);

                    query = deptQuery.Union(rangeQuery);
                }
                else
                {
                    var additionalQuery = _context.Coa
                        .Where(x => x.gl_kode.Trim() == "16010100");

                    query = deptQuery.Union(additionalQuery);
                }
            }

            // Filter pencarian
            if (!string.IsNullOrWhiteSpace(request.SearchKeyword))
            {
                string keyword = request.SearchKeyword.ToLower();

                query = query.Where(x =>
                    x.gl_kode.ToLower().Contains(keyword) ||
                    x.gl_nama.ToLower().Contains(keyword) ||
                    x.gl_dept.ToLower().Contains(keyword) ||
                    x.gl_type.ToLower().Contains(keyword));
            }

            return await query
                .ProjectTo<CoaDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}