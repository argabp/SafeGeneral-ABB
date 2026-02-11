using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ABB.Application.KasBanks.Queries
{
    public class GetNextKodeKasBankQuery : IRequest<string>
    {
        public string KodeCabang { get; set; }
        public string TipeKasBank { get; set; }
    }

    public class GetNextKodeKasBankQueryHandler 
        : IRequestHandler<GetNextKodeKasBankQuery, string>
    {
        private readonly IDbContextPstNota _context;

        public GetNextKodeKasBankQueryHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<string> Handle(
            GetNextKodeKasBankQuery request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.KodeCabang) ||
                string.IsNullOrEmpty(request.TipeKasBank))
            {
                return "";
            }

            // Ambil kode terakhir berdasarkan Cabang + Tipe
            var lastKode = await _context.KasBank
                .Where(x =>
                    x.KodeCabang == request.KodeCabang &&
                    x.TipeKasBank == request.TipeKasBank)
                .OrderByDescending(x => x.Kode)
                .Select(x => x.Kode)
                .FirstOrDefaultAsync(cancellationToken);

            int nextKode = 1;

            if (!string.IsNullOrEmpty(lastKode) &&
                int.TryParse(lastKode, out int lastNumber))
            {
                nextKode = lastNumber + 1;
            }

            // Format 2 digit → 01, 02, 03, 14, dst
            return nextKode.ToString("D2");
        }
    }
}
