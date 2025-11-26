using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ABB.Application.KasBanks.Queries
{
    // Ini adalah "surat permintaan" untuk mendapatkan semua data KasBank
    public class GetAllKasBankQuery : IRequest<List<KasBankDto>>
    {
        // Biasanya kosong karena kita tidak butuh parameter apapun untuk "get all"
        public string SearchKeyword { get; set; }
        public string TipeKasBank { get; set; }
    }

    // Ini adalah "eksekutor" yang akan menjalankan permintaan di atas
    public class GetAllKasBankQueryHandler : IRequestHandler<GetAllKasBankQuery, List<KasBankDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllKasBankQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<KasBankDto>> Handle(GetAllKasBankQuery request, CancellationToken cancellationToken)
        {
            // Ambil data dasar
            var query = _context.KasBank.AsQueryable();

            // JIKA ADA KATA KUNCI, LAKUKAN FILTER
            if (!string.IsNullOrEmpty(request.SearchKeyword))
            {
                var keyword = request.SearchKeyword.ToLower();
                query = query.Where(kb => 
                    kb.Kode.ToLower().Contains(keyword) ||
                    kb.Keterangan.ToLower().Contains(keyword) ||
                    kb.NoRekening.ToLower().Contains(keyword)
                );
            }
            if (!string.IsNullOrEmpty(request.TipeKasBank))
            {
                query = query.Where(kb => kb.TipeKasBank == request.TipeKasBank);
            }

            // Lanjutkan proses seperti biasa dengan data yang sudah difilter
            var kasBankList = await query
                .ProjectTo<KasBankDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return kasBankList;
        }
    }
}