using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ABB.Application.EntriPenyelesaianPiutangs.Queries;

namespace ABB.Application.PostingPenyelesaianPiutang.Queries
{
    public class GetAllPenyelesaianPiutangByFlagQuery : IRequest<List<HeaderPenyelesaianUtangDto>>
    {
        public bool FlagPosting { get; set; }   // true = sudah posting, false = belum posting
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }   
        public string KodeCabang { get; set; }
    }

   public class GetAllPenyelesaianPiutangByFlagQueryHandler : IRequestHandler<GetAllPenyelesaianPiutangByFlagQuery, List<HeaderPenyelesaianUtangDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        // 🎯 DIPERBAIKI: Nama constructor harus sama dengan nama class
        public GetAllPenyelesaianPiutangByFlagQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<HeaderPenyelesaianUtangDto>> Handle(GetAllPenyelesaianPiutangByFlagQuery request, CancellationToken cancellationToken)
        {
            // Ambil data dasar dengan filter FlagPosting
            var query = _context.HeaderPenyelesaianUtang // 💡 Gunakan DbSet yang benar
                .Where(h => h.FlagPosting == request.FlagPosting)  // filter berdasarkan flag
                .AsQueryable();
            if (!string.IsNullOrEmpty(request.KodeCabang))
            {
                query = query.Where(x => x.KodeCabang == request.KodeCabang);
            }
            // Jika ada kata kunci pencarian, filter lagi
            if (!string.IsNullOrEmpty(request.SearchKeyword))
            {
                var keyword = request.SearchKeyword.ToLower();
                decimal searchDecimal;
                bool isDecimal = decimal.TryParse(request.SearchKeyword, out searchDecimal);

                // 🎯 DIPERBAIKI: Gunakan properti yang ada di HeaderPenyelesaianUtang
                query = query.Where(h =>
                    h.KodeCabang.ToLower().Contains(keyword) ||
                    h.JenisPenyelesaian.ToLower().Contains(keyword) || // Diganti dari JenisVoucher
                    h.NomorBukti.ToLower().Contains(keyword) ||       // Diganti dari NoVoucher
                    h.KodeAkun.ToLower().Contains(keyword) ||
                    // Diganti dari TotalVoucher, sesuaikan dengan kolom yang ingin dicari (TotalRp atau TotalOrg)
                    (isDecimal && h.TotalRp.HasValue && h.TotalRp.Value == searchDecimal)
                );
            }

            // 🎯 DIPERBAIKI: Proyeksikan ke DTO yang benar
            var headerList = await query
                .ProjectTo<HeaderPenyelesaianUtangDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return headerList;
        }
    }
}
