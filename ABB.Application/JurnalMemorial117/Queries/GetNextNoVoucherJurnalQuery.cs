using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.JurnalMemorial117.Queries
{
    public class GetNextNoVoucherJurnalQuery : IRequest<string>
    {
        public string KodeCabang { get; set; }
        public int Bulan { get; set; }
        public int Tahun { get; set; } // 4 digit (misal 2025)
    }

    public class GetNextNoVoucherJurnalQueryHandler
        : IRequestHandler<GetNextNoVoucherJurnalQuery, string>
    {
        private readonly IDbContextPstNota _context;

        public GetNextNoVoucherJurnalQueryHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<string> Handle(
            GetNextNoVoucherJurnalQuery request,
            CancellationToken cancellationToken)
        {
            // =========================
            // 1. Ambil 2 digit terakhir Kode Cabang (Trim + Safe)
            // =========================
            string kodeCabang = request.KodeCabang?.Trim() ?? "";
            string kodeCabang2Digit = kodeCabang.Length >= 2
                ? kodeCabang[^2..]
                : kodeCabang.PadLeft(2, '0');

            // =========================
            // 2. Komponen Nomor Voucher
            // =========================
            string jenis = "MM";
            string bulanDuaDigit = request.Bulan.ToString("D2");
            string tahunFull = request.Tahun.ToString("D4");

            // Prefix: CC/MM/BB/YYYY/
            string prefix =
                $"{kodeCabang2Digit}/" +
                $"{jenis}/" +
                $"{bulanDuaDigit}/" +
                $"{tahunFull}/";

            // =========================
            // 3. Cari nomor terakhir (reset per bulan & tahun)
            // =========================
            var lastVoucher = await _context.JurnalMemorial117
                .Where(x =>
                    x.KodeCabang.Trim() == kodeCabang &&
                    x.NoVoucher.StartsWith(prefix))
                .OrderByDescending(x => x.NoVoucher)
                .Select(x => x.NoVoucher)
                .FirstOrDefaultAsync(cancellationToken);

            int nextUrut = 1;

            if (!string.IsNullOrEmpty(lastVoucher))
            {
                // Contoh: 01/MM/02/2025/007
                var lastUrutStr = lastVoucher.Split('/').Last();

                if (int.TryParse(lastUrutStr, out int lastUrut))
                {
                    nextUrut = lastUrut + 1;
                }
            }

            // =========================
            // 4. Hasil Akhir
            // =========================
            return $"{prefix}{nextUrut:D3}";
        }
    }
}
