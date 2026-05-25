using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.Coas117.Queries
{
    public class GetAllCoa117Query : IRequest<List<Coa117Dto>>
    {
        public string SearchKeyword { get; set; } // ✅ properti pencarian
         public string KodeCabang { get; set; }
    }

    public class GetAllCoa117QueryHandler : IRequestHandler<GetAllCoa117Query, List<Coa117Dto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllCoa117QueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Coa117Dto>> Handle(GetAllCoa117Query request, CancellationToken cancellationToken)
        {
            var query = _context.Coa117.AsQueryable();

             if (!string.IsNullOrWhiteSpace(request.KodeCabang))
            {
                string dept = request.KodeCabang.Trim();

                var deptQuery = _context.Coa117
                    .Where(x => x.gl_dept.Trim() == dept);

                if (dept == "10")
                {
                    var rangeQuery = _context.Coa117
                        .Where(x => 
                            x.gl_kode.Trim().CompareTo("16011100") >= 0 &&
                            x.gl_kode.Trim().CompareTo("16070100") <= 0);

                    query = deptQuery.Union(rangeQuery);
                }
                else
                {
                    var additionalQuery = _context.Coa117
                        .Where(x => x.gl_kode.Trim() == "16010100");

                    query = deptQuery.Union(additionalQuery);
                }
            }


            // ✅ Tambahkan filter pencarian
            if (!string.IsNullOrWhiteSpace(request.SearchKeyword))
            {
                string keyword = request.SearchKeyword.ToLower();
                query = query.Where(x =>
                    x.gl_kode.ToLower().Contains(keyword) ||
                    x.gl_nama.ToLower().Contains(keyword) ||
                    x.gl_dept.ToLower().Contains(keyword) ||
                    x.gl_type.ToLower().Contains(keyword));
            }

            // ✅ Project ke DTO menggunakan AutoMapper
            return await query
                .ProjectTo<Coa117Dto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
