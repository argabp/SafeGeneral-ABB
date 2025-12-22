using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.JurnalMemorials104.Queries
{
    public class GetNextNoVoucherJurnalQuery : IRequest<string>
    {
        public string KodeCabang { get; set; }
        public int Bulan { get; set; }
        public int Tahun { get; set; } // Kirim 4 digit (misal: 2025)
    }

    public class GetNextNoVoucherJurnalQueryHandler : IRequestHandler<GetNextNoVoucherJurnalQuery, string>
    {
        private readonly IDbContextPstNota _context;

        public GetNextNoVoucherJurnalQueryHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<string> Handle(GetNextNoVoucherJurnalQuery request, CancellationToken cancellationToken)
        {
            // 1. Tentukan format suffix: /MM/02/25
            // MM = Default (sesuai request)
            // BB = request.Bulan (2 digit)
            // TT = request.Tahun (ambil 2 digit belakang)
            string tahunDuaDigit = (request.Tahun % 100).ToString("D2");
            string bulanDuaDigit = request.Bulan.ToString("D2");
            string jenis = "MM"; 

            // Format yang dicari di database: "%/MM/02/25"
            string searchPattern = $"/{jenis}/{bulanDuaDigit}/{tahunDuaDigit}";

            // 2. Cari nomor terakhir di database berdasarkan Cabang & Suffix Bulan/Tahun ini
            // Kita pakai EndsWith untuk memastikan bulan & tahunnya sama (agar reset tiap bulan)
            var lastVoucher = await _context.JurnalMemorial104
                .Where(x => x.KodeCabang == request.KodeCabang && 
                            x.NoVoucher.EndsWith(searchPattern))
                .OrderByDescending(x => x.NoVoucher) // Urutkan Z-A untuk dapat yang terbesar
                .Select(x => x.NoVoucher)
                .FirstOrDefaultAsync(cancellationToken);

            int nextUrut = 1;

            if (!string.IsNullOrEmpty(lastVoucher))
            {
                // Contoh lastVoucher: "005/MM/02/25"
                // Kita split berdasarkan '/' ambil index ke-0
                var parts = lastVoucher.Split('/');
                
                if (parts.Length > 0 && int.TryParse(parts[0], out int currentUrut))
                {
                    nextUrut = currentUrut + 1;
                }
            }

            // 3. Rakit Nomor Baru: "001" + "/MM/02/25"
            string newVoucher = $"{nextUrut.ToString("D3")}{searchPattern}";

            return newVoucher;
        }
    }
}