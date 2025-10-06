using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPembayaranKass.Queries
{
    public class GetNotaProduksiQuery : IRequest<List<NotaProduksiDto>> { }

    public class GetNotaProduksiQueryHandler : IRequestHandler<GetNotaProduksiQuery, List<NotaProduksiDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetNotaProduksiQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<NotaProduksiDto>> Handle(GetNotaProduksiQuery request, CancellationToken cancellationToken)
        {
            return await _context.Produksi
                .ProjectTo<NotaProduksiDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}