using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.JurnalMemorial117.Queries
{
    public class GetJurnalMemorial117ByFlagQuery : IRequest<List<JurnalMemorial117Dto>>
    {
        public bool FlagPosting { get; set; }   // true = sudah posting, false = belum posting
        public string SearchKeyword { get; set; }
        public string KodeCabang { get; set; } 
    }

    public class GetJurnalMemorial117ByFlagQueryHandler : IRequestHandler<GetJurnalMemorial117ByFlagQuery, List<JurnalMemorial117Dto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetJurnalMemorial117ByFlagQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<JurnalMemorial117Dto>> Handle(GetJurnalMemorial117ByFlagQuery request, CancellationToken cancellationToken)
        {
            // 1. Ambil data dasar dari tabel JurnalMemorial117
            var query = _context.JurnalMemorial117.AsQueryable();

            // 2. Filter Cabang (Wajib)
            if (!string.IsNullOrEmpty(request.KodeCabang))
            {
                query = query.Where(h => h.KodeCabang == request.KodeCabang);
            }

            // 3. Filter Flag Posting
            if (request.FlagPosting == false)
            {
                query = query.Where(h => h.FlagPosting == false || h.FlagPosting == null);
            }
            else
            {
                query = query.Where(h => h.FlagPosting == true);
            }

            // --- 4. VALIDASI SIAP POSTING (Has Detail & Balance) ---
            // Hanya jalankan filter ini jika kita sedang mencari data yang BELUM posting.
            // (Data yang sudah posting diasumsikan sudah valid/balance).
          if (request.FlagPosting == false)
            {
                query = query.Where(h => 
                    // A. Cek Apakah Punya Detail
                    _context.JurnalMemorial117Detail.Any(d => d.NoVoucher == h.NoVoucher) 
                    
                    && // DAN
                    
                    // B. Cek Balance (Total Debet == Total Kredit)
                    (
                        _context.JurnalMemorial117Detail
                            .Where(d => d.NoVoucher == h.NoVoucher)
                            .Sum(d => d.NilaiDebet) 
                        == 
                        _context.JurnalMemorial117Detail
                            .Where(d => d.NoVoucher == h.NoVoucher)
                            .Sum(d => d.NilaiKredit)
                    )
                );
            }

            // 5. Filter Search Keyword
            if (!string.IsNullOrEmpty(request.SearchKeyword))
            {
                var keyword = request.SearchKeyword.ToLower();
                
                query = query.Where(h =>
                    h.NoVoucher.ToLower().Contains(keyword) ||       
                    (h.Keterangan != null && h.Keterangan.ToLower().Contains(keyword)) 
                );
            }

            // 6. Proyeksikan ke DTO
            var resultList = await query
                .OrderByDescending(h => h.Tanggal) // Urutkan tanggal terbaru
                .ProjectTo<JurnalMemorial117Dto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return resultList;
        }
    }
}