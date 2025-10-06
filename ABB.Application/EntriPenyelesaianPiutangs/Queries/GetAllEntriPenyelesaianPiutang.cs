using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPenyelesaianPiutangs.Queries
{
    public class GetAllEntriPenyelesaianPiutangQuery : IRequest<List<EntriPenyelesaianPiutangDto>> 
    {
        // TAMBAHKAN PROPERTI INI
        public string SearchKeyword { get; set; }
        public string NoBukti { get; set; }
    }

    public class GetAllEntriPenyelesaianPiutangQueryHandler : IRequestHandler<GetAllEntriPenyelesaianPiutangQuery, List<EntriPenyelesaianPiutangDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllEntriPenyelesaianPiutangQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<EntriPenyelesaianPiutangDto>> Handle(GetAllEntriPenyelesaianPiutangQuery request, CancellationToken cancellationToken)
        {
            // Ambil data dasar
            var query = _context.EntriPenyelesaianPiutang.AsQueryable();

            // TAMBAHKAN LOGIKA FILTER DI SINI
            if (!string.IsNullOrEmpty(request.SearchKeyword))
            {
                var keyword = request.SearchKeyword.ToLower();
                query = query.Where(pb => 
                    pb.NoBukti.ToLower().Contains(keyword) ||
                    pb.KodeAkun.ToLower().Contains(keyword)
                );
            }

            if (!string.IsNullOrEmpty(request.NoBukti))
            {
                query = query.Where(pb => pb.NoBukti == request.NoBukti);
            }
            // ------------------------------------

            return await query
                .ProjectTo<EntriPenyelesaianPiutangDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}