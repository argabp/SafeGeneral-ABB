using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ABB.Application.JurnalMemorials104.Queries;

namespace ABB.Application.CancelPostingJurnalMemorial104.Queries
{
    public class GetAllCancelJurnalMemorial104Query : IRequest<List<JurnalMemorial104Dto>>
    {
         public bool FlagGL { get; set; }
        public string SearchKeyword { get; set; }
        public string KodeCabang { get; set; }   // ✅ WAJIB DITAMBAH
    }

    public class GetAllCancelJurnalMemorial104QueryHandler : IRequestHandler<GetAllCancelJurnalMemorial104Query, List<JurnalMemorial104Dto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllCancelJurnalMemorial104QueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<JurnalMemorial104Dto>> Handle(GetAllCancelJurnalMemorial104Query request, CancellationToken cancellationToken)
        {
            // Ambil data dasar dengan filter FlagPosting
           var query = _context.JurnalMemorial104
            .Where(v => v.FlagGL == request.FlagGL
                    && v.KodeCabang == request.KodeCabang)
            .AsQueryable();

            // Jika ada kata kunci pencarian, filter lagi
            if (!string.IsNullOrEmpty(request.SearchKeyword))
            {
                var keyword = request.SearchKeyword.ToLower();
                decimal searchDecimal;
                bool isDecimal = decimal.TryParse(request.SearchKeyword, out searchDecimal);

                query = query.Where(kb =>
                    kb.KodeCabang.ToLower().Contains(keyword) ||
                    kb.NoVoucher.ToLower().Contains(keyword) 
                );
            }

            // Proyeksikan ke DTO
            var JurnalMemorial104List = await query
                .ProjectTo<JurnalMemorial104Dto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return JurnalMemorial104List;
        }
    }
}
