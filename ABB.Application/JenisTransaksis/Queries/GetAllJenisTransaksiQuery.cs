using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.JenisTransaksis.Queries
{
    public class GetAllJenisTransaksiQuery : IRequest<List<JenisTransaksiDto>>
    {
        
    }

    public class GetAllJenisTransaksiQueryHandler : IRequestHandler<GetAllJenisTransaksiQuery, List<JenisTransaksiDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllJenisTransaksiQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<JenisTransaksiDto>> Handle(GetAllJenisTransaksiQuery request, CancellationToken cancellationToken)
        {
          var query = _context.JenisTransaksi.AsQueryable();

            return await query
                .OrderByDescending(x => x.id)
                .ProjectTo<JenisTransaksiDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}