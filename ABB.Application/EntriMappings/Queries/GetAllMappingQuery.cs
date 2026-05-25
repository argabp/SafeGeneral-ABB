using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriMappings.Queries
{
    public class GetAllMappingQuery : IRequest<List<EntriMappingDto>>
    {
        public string SearchKeyword { get; set; } 
    }

    public class GetAllMappingQueryHandler : IRequestHandler<GetAllMappingQuery, List<EntriMappingDto>>
    {
        private readonly IDbContextPstNota _context; // Sesuaikan nama Context Anda
        private readonly IMapper _mapper;

        public GetAllMappingQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

       public async Task<List<EntriMappingDto>> Handle(GetAllMappingQuery request, CancellationToken cancellationToken)
        {
            var query = from m in _context.EntriMapping
                        join c104 in _context.Coa on m.gl_akun104 equals c104.gl_kode into gj104
                        from c104 in gj104.DefaultIfEmpty()
                        join c117 in _context.Coa117 on m.gl_akun117 equals c117.gl_kode into gj117
                        from c117 in gj117.DefaultIfEmpty()
                        select new EntriMappingDto
                        {
                            gl_akun104 = m.gl_akun104,
                            Nama104 = c104 != null ? c104.gl_nama : "",
                            gl_akun117 = m.gl_akun117,
                            Nama117 = c117 != null ? c117.gl_nama : ""
                        };

            if (!string.IsNullOrEmpty(request.SearchKeyword))
            {
                string keyword = request.SearchKeyword.ToLower();
                query = query.Where(x => x.gl_akun104.Contains(keyword) || x.gl_akun117.Contains(keyword));
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}