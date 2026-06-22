using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.ProsesTutupTahun.Dtos; 
using ABB.Domain.Entities; 
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.ProsesTutupTahun.Queries
{
    public class GetDaftarPeriodeTahunQuery : IRequest<List<ProsesTutupTahunDto>> { }

    public class GetDaftarPeriodeTahunQueryHandler : IRequestHandler<GetDaftarPeriodeTahunQuery, List<ProsesTutupTahunDto>>
    {
        private readonly IDbContextPstNota _context;

        public GetDaftarPeriodeTahunQueryHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<List<ProsesTutupTahunDto>> Handle(GetDaftarPeriodeTahunQuery request, CancellationToken cancellationToken)
        {
            // Ambil daftar tahun unik dari EntriPeriode (Tabel ac03)
            var tahunList = await _context.EntriPeriode
                .Where(x => x.ThnPrd != null)
                .Select(x => x.ThnPrd)
                .Distinct()
                .OrderByDescending(x => x)
                .ToListAsync(cancellationToken);

            var result = new List<ProsesTutupTahunDto>();

            foreach (var t in tahunList)
            {
                // [PERBAIKAN SAKTI]: Daripada cuma nge-Count, kita ambil daftar bulannya langsung!
                var bulanBelumTutupList = await _context.EntriPeriode
                    .Where(x => x.ThnPrd == t && x.BlnPrd >= 1 && x.BlnPrd <= 12 && x.FlagClosing != "N")
                    .Select(x => x.BlnPrd)
                    .OrderBy(x => x) // Urutkan bulannya dari 1 s/d 12
                    .ToListAsync(cancellationToken);

                // Cek apakah tahun depannya sudah dibuat (Sebagai tanda sudah tutup tahun)
                var sudahTutupTahun = await _context.EntriPeriode
                    .AnyAsync(x => x.ThnPrd == (t + 1), cancellationToken); 

                if (sudahTutupTahun)
                {
                    // Kalau sudah ditutup tahunnya, tidak usah muncul di Grid
                    continue; 
                }

                // Jika list bulan belum tutup itu KOSONG, artinya semua beres
                if (!bulanBelumTutupList.Any())
                {
                    result.Add(new ProsesTutupTahunDto { Tahun = (int)t, Status = "Open" });
                }
                else
                {
                    // Gabungkan list angka bulan pakai koma (Contoh: "1, 2, 3")
                    string daftarBulan = string.Join(", ", bulanBelumTutupList);
                    
                    // Tempel ke Status
                    result.Add(new ProsesTutupTahunDto { 
                        Tahun = (int)t, 
                        Status = $"Bulan Belum Close (Bln: {daftarBulan})" 
                    });
                }
            }
            
            return result;
        }
    }
}