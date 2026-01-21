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
        public string TipeLaporan { get; set; } 
        
        // [TAMBAHKAN INI] Agar Controller tidak error
        public string SearchKeyword { get; set; } 
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

            // Filter Tipe Laporan (Opsional)
            if (!string.IsNullOrEmpty(request.TipeLaporan))
            {
                query = query.Where(x => x.TipeLaporan == request.TipeLaporan);
            }

            // [TAMBAHKAN LOGIC PENCARIAN INI]
            if (!string.IsNullOrEmpty(request.SearchKeyword))
            {
                var lowerKeyword = request.SearchKeyword.ToLower();
                query = query.Where(x => 
                    x.Deskripsi.ToLower().Contains(lowerKeyword) || 
                    x.Rumus.ToLower().Contains(lowerKeyword) ||
                    x.TipeLaporan.ToLower().Contains(lowerKeyword)
                );
            }

            return await query
                .OrderBy(x => x.Id) // Urutkan berdasarkan ID agar susunan baris rapi
                .ProjectTo<TemplateLapKeuDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}