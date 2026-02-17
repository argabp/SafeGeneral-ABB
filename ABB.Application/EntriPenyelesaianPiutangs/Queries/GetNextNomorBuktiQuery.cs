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
            string kodeCabang = request.KodeCabang?.Trim() ?? "";

            string kodeCabang2Digit = kodeCabang.Length >= 2
                ? kodeCabang[^2..]
                : kodeCabang.PadLeft(2, '0');
            string prefix =
                $"{kodeCabang2Digit}/" +
                $"{request.JenisPenyelesaian}/" +
                $"{request.Bulan:D2}/" +
                $"{request.Tahun:D2}/";

          //  string suffix = $"/{request.JenisPenyelesaian}/{request.Bulan:D2}/{request.Tahun:D2}";

            // 2. Cari nomor bukti terakhir yang cocok dengan akhiran bulan ini
           var lastNomorBukti = await _context.HeaderPenyelesaianUtang
                .Where(v => v.NomorBukti.StartsWith(prefix))
                .OrderByDescending(v => v.NomorBukti)
                .Select(v => v.NomorBukti)
                .FirstOrDefaultAsync(cancellationToken);

            int nextSequence = 1;
           if (!string.IsNullOrEmpty(lastNomorBukti))
            {
                // =========================
                // 4. Ambil NNN (bagian terakhir)
                // =========================
                var lastSequenceStr = lastNomorBukti.Split('/').Last();

                if (int.TryParse(lastSequenceStr, out int lastSequence))
                {
                    nextSequence = lastSequence + 1;
                }
            }
            // 4. Gabungkan nomor urut baru dengan format yang benar (3 digit)
            // Format hasil: 002/BM/10/25
            return $"{prefix}{nextSequence:D3}";
        }
    }
}