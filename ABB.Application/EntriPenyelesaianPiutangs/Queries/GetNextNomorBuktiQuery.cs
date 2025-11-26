using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPenyelesaianPiutangs.Queries
{
    public class GetNextNomorBuktiQuery : IRequest<string>
    {
        public string KodeCabang { get; set; }
        public string JenisPenyelesaian { get; set; } // e.g., "BM"
        public int Bulan { get; set; }
        public int Tahun { get; set; } // 2 digit, e.g., 25
    }

    public class GetNextNomorBuktiQueryHandler : IRequestHandler<GetNextNomorBuktiQuery, string>
    {
        private readonly IDbContextPstNota _context;
        public GetNextNomorBuktiQueryHandler(IDbContextPstNota context)
        {
            _context = context;
        }

       public async Task<string> Handle(GetNextNomorBuktiQuery request, CancellationToken cancellationToken)
        {
            // 1. Bentuk akhiran (suffix) untuk pencarian yang lebih akurat
            // Format: /BM/10/25
            string suffix = $"/{request.JenisPenyelesaian}/{request.Bulan:D2}/{request.Tahun:D2}";

            // 2. Cari nomor bukti terakhir yang cocok dengan akhiran bulan ini
            var lastNomorBukti = await _context.HeaderPenyelesaianUtang
                .Where(v => v.NomorBukti.EndsWith(suffix))
                .OrderByDescending(v => v.NomorBukti) // Mengurutkan berdasarkan string akan bekerja untuk format ini
                .Select(v => v.NomorBukti) // Ambil string-nya saja
                .FirstOrDefaultAsync(cancellationToken);

            int nextSequence = 1;
            if (lastNomorBukti != null)
            {
                // 3. Ambil bagian PERTAMA (nomor urut), bukan terakhir
                var lastSequenceStr = lastNomorBukti.Split('/').First();
                if (int.TryParse(lastSequenceStr, out int lastSequence))
                {
                    nextSequence = lastSequence + 1;
                }
            }

            // 4. Gabungkan nomor urut baru dengan format yang benar (3 digit)
            // Format hasil: 002/BM/10/25
            return $"{nextSequence:D3}{suffix}";
        }
    }
}