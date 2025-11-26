using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ABB.Application.CancelPostingPenyelesaianPiutang.Queries;
using ABB.Application.CancelPostingPenyelesaianPiutang.Commands;
using ABB.Application.EntriPenyelesaianPiutangs.Queries;

namespace ABB.Application.CancelPostingPenyelesaianPiutang.Queries
{
    public class GetAllPostingPenyelesaianPiutangQueryByFlagCancel : IRequest<List<HeaderPenyelesaianUtangDto>>
    {
        public bool FlagPosting { get; set; }   // true = sudah posting, false = belum posting
        public string SearchKeyword { get; set; }
         public string DatabaseName { get; set; }   
    }

    public class GetAllPenyelesaianPiutangByFlagCancelQueryHandler : IRequestHandler<GetAllPostingPenyelesaianPiutangQueryByFlagCancel, List<HeaderPenyelesaianUtangDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllPenyelesaianPiutangByFlagCancelQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<HeaderPenyelesaianUtangDto>> Handle(GetAllPostingPenyelesaianPiutangQueryByFlagCancel request, CancellationToken cancellationToken)
        {
            // Ambil data dasar dengan filter FlagPosting
            var query = _context.HeaderPenyelesaianUtang
                .Where(v => v.FlagPosting == request.FlagPosting)  // filter berdasarkan flag
                .AsQueryable();

            // Jika ada kata kunci pencarian, filter lagi
            if (!string.IsNullOrEmpty(request.SearchKeyword))
            {
                var keyword = request.SearchKeyword.ToLower();
                decimal searchDecimal;
                bool isDecimal = decimal.TryParse(request.SearchKeyword, out searchDecimal);

                query = query.Where(kb =>
                    kb.KodeCabang.ToLower().Contains(keyword) ||
                    kb.JenisPenyelesaian.ToLower().Contains(keyword) || // Diganti dari JenisVoucher
                    kb.NomorBukti.ToLower().Contains(keyword) ||       // Diganti dari NoVoucher
                    kb.KodeAkun.ToLower().Contains(keyword) ||
                    (isDecimal && kb.TotalRp.HasValue && kb.TotalRp.Value == searchDecimal)
                );
            }

            // Proyeksikan ke DTO
            var headerPiutangList = await query
                .ProjectTo<HeaderPenyelesaianUtangDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return headerPiutangList;
        }
    }
}
