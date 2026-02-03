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
        public string SearchKeyword { get; set; } // ✅ properti pencarian
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
                string dept = request.KodeCabang.Trim(); // Bersihkan spasi input

                // Gunakan Trim() pada database field jika tipe datanya CHAR/NCHAR
                query = query.Where(x => x.gl_dept.Trim() == dept);
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
                .ProjectTo<CoaDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
