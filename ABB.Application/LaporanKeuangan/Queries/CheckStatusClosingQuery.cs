using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.LaporanKeuangan.Queries
{
    // =========================================================================
    // QUERY & HANDLER UNTUK VALIDASI STATUS CLOSING
    // =========================================================================
    
    public class CheckStatusClosingQuery : IRequest<bool>
    {
        public int Bulan { get; set; }
        public int Tahun { get; set; }

        public CheckStatusClosingQuery(int bulan, int tahun)
        {
            Bulan = bulan;
            Tahun = tahun;
        }
    }

    public class CheckStatusClosingQueryHandler : IRequestHandler<CheckStatusClosingQuery, bool>
    {
        private readonly IDbContextPstNota _context;

        public CheckStatusClosingQueryHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CheckStatusClosingQuery request, CancellationToken cancellationToken)
        {
            // 1. Ambil data dari tabel EntriPeriode
            // Berdasarkan instruksi: 'Y' = Belum Close, 'N' = Sudah Close
            var periode = await _context.EntriPeriode
                .AsNoTracking()
                .Where(x => x.BlnPrd == request.Bulan && x.ThnPrd == request.Tahun)
                .FirstOrDefaultAsync(cancellationToken);

            // 2. Jika data tidak ditemukan, kita asumsikan belum aman (Belum Closing)
            if (periode == null)
            {
                return false; 
            }

            // 3. Cek Status
            // Kita gunakan Trim() dan ToUpper() untuk menghindari error karakter
            string status = (periode.FlagClosing ?? "").Trim().ToUpper();

            if (status == "N")
            {
                // Sudah Closing
                return true;
            }
            else
            {
                // Jika 'Y' atau status lainnya, berarti belum closing
                return false;
            }
        }
    }
}