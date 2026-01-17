using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ABB.Domain.Entities;

namespace ABB.Application.TemplateLapKeus.Queries
{
    public class GetAllTemplateLapKeuQuery : IRequest<List<TemplateLapKeuDto>>
    {
        public string TipeLaporan { get; set; } // Filter optional: NR / LR
    }

    public class GetAllTemplateLapKeuQueryHandler : IRequestHandler<GetAllTemplateLapKeuQuery, List<TemplateLapKeuDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllTemplateLapKeuQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TemplateLapKeuDto>> Handle(GetAllTemplateLapKeuQuery request, CancellationToken cancellationToken)
        {
            var query = _context.TemplateLapKeu.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.TipeLaporan))
            {
                query = query.Where(x => x.TipeLaporan == request.TipeLaporan);
            }

            // Urutkan berdasarkan ID (asumsi ID merepresentasikan urutan baris)
            // Atau nanti bisa tambah kolom 'Urutan' int jika butuh insert di tengah2
            return await query
                .OrderBy(x => x.Id)
                .ProjectTo<TemplateLapKeuDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}