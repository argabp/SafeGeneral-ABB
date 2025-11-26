using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.InquiryNotaProduksis.Queries
{
    public class GetDistinctJenisAssetQuery : IRequest<List<string>>
    {
    }

    public class GetDistinctJenisAssetQueryHandler : IRequestHandler<GetDistinctJenisAssetQuery, List<string>>
    {
        private readonly IDbContextPstNota _context;

        public GetDistinctJenisAssetQueryHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<List<string>> Handle(GetDistinctJenisAssetQuery request, CancellationToken cancellationToken)
        {
            // Ambil nilai distinct dari kolom jn_ass di tabel Produksi
            var result = await _context.Produksi
                .Where(p => !string.IsNullOrEmpty(p.jn_ass))
                .Select(p => p.jn_ass)
                .Distinct()
                .OrderBy(p => p)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
