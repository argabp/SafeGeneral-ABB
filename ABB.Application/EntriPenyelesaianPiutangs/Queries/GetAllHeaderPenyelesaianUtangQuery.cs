using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace ABB.Application.EntriPenyelesaianPiutangs.Queries // <-- Namespace sudah benar
{
    public class GetAllHeaderPenyelesaianUtangQuery : IRequest<List<HeaderPenyelesaianUtangDto>>
    {
         public string SearchKeyword { get; set; }
    }

    public class GetAllHeaderPenyelesaianUtangQueryHandler : IRequestHandler<GetAllHeaderPenyelesaianUtangQuery, List<HeaderPenyelesaianUtangDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllHeaderPenyelesaianUtangQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<HeaderPenyelesaianUtangDto>> Handle(GetAllHeaderPenyelesaianUtangQuery request, CancellationToken cancellationToken)
        {
            var query = _context.HeaderPenyelesaianUtang.AsQueryable();

            // JIKA ADA KATA KUNCI, LAKUKAN FILTER
            if (!string.IsNullOrEmpty(request.SearchKeyword))
            {
                var keyword = request.SearchKeyword.ToLower();

                // --- FIX: Gunakan properti yang benar dari HeaderPenyelesaianUtang ---
                query = query.Where(h =>
                    h.NomorBukti.ToLower().Contains(keyword) ||
                    h.Keterangan.ToLower().Contains(keyword) ||
                    h.KodeAkun.ToLower().Contains(keyword)
                );
            }

            // --- FIX: ProjectTo DTO yang benar ---
            return await query
                .ProjectTo<HeaderPenyelesaianUtangDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}