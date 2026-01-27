using System.Threading.Tasks;
using System.Linq;
using ABB.Application.TemplateLapKeus.Commands;
using ABB.Application.TemplateLapKeus.Queries;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using ABB.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ABB.Web.Modules.TemplateLapKeu
{
    public class TemplateLapKeuController : AuthorizedBaseController
    {
       
        private readonly IDbContextPstNota _context;

        public TemplateLapKeuController(IDbContextPstNota context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetGridData([DataSourceRequest] DataSourceRequest request, string searchKeyword, string tipeLaporan)
        {
            // Kirim searchKeyword ke Query Handler
            var data = await Mediator.Send(new GetAllTemplateLapKeuQuery { SearchKeyword = searchKeyword, TipeLaporan = tipeLaporan }); 
            return Json(await data.ToDataSourceResultAsync(request));
        }

        public async Task<IActionResult> Add(long? id, string tipeLaporan = "") // <--- Tambah parameter ini
        {
            var dto = new TemplateLapKeuDto();

            if (id.HasValue && id.Value > 0)
            {
                dto = await Mediator.Send(new GetTemplateLapKeuByIdQuery { Id = id.Value });
            }
            else 
            {
                // Set Default Tipe Laporan dari Tab yang aktif
                dto.TipeLaporan = tipeLaporan; 
            }

            return PartialView("_Add", dto);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] TemplateLapKeuDto model)
        {
            // LOGIKA 1: AUTO NUMBER (Jika user tidak isi / 0)
            if (model.Urutan == 0)
            {
                // PERBAIKAN: Tambah Filter TipeLaporan
                // Jadi Max Urutan hanya dihitung dari Tipe Laporan yang bersangkutan (misal: NERACA aja)
                var maxUrutan = await _context.TemplateLapKeu
                    .Where(x => x.TipeLaporan == model.TipeLaporan) 
                    .MaxAsync(x => (int?)x.Urutan) ?? 0;
                
                model.Urutan = maxUrutan + 1;
            }
            else
            {
                // LOGIKA 2: SISIP DATA (INSERT SHIFT)
                // Cek apakah urutan yang diminta user SUDAH ADA ?
                // PERBAIKAN: Tambah Filter TipeLaporan
                // Cek bentrok hanya di Tipe Laporan yang sama. 
                // (Urutan 5 di NERACA tidak akan bentrok dengan Urutan 5 di LABARUGI)
                var isBentrok = await _context.TemplateLapKeu
                    .AnyAsync(x => x.Urutan == model.Urutan 
                                   && x.Id != model.Id 
                                   && x.TipeLaporan == model.TipeLaporan); 

                if (isBentrok)
                {
                    // AMBIL SEMUA TETANGGA YANG POSISINYA >= URUTAN BARU
                    // PERBAIKAN: Tambah Filter TipeLaporan
                    // Hanya geser tetangga yang satu Tipe Laporan
                    var tetanggaYgHarusGeser = await _context.TemplateLapKeu
                        .Where(x => x.Urutan >= model.Urutan && x.TipeLaporan == model.TipeLaporan)
                        .OrderBy(x => x.Urutan)
                        .ToListAsync();

                    // LAKUKAN PERGESERAN (+1)
                    foreach (var row in tetanggaYgHarusGeser)
                    {
                        row.Urutan += 1; 
                    }

                    // SIMPAN PERGESERAN DULU KE DATABASE
                    _context.TemplateLapKeu.UpdateRange(tetanggaYgHarusGeser);
                    
                    // Tambahkan: System.Threading.CancellationToken.None
                    await _context.SaveChangesAsync(System.Threading.CancellationToken.None);
                }
            }

            // LOGIKA 3: SIMPAN DATA UTAMA (Create / Update)
            if (model.Id > 0)
            {
                var command = new UpdateTemplateLapKeuCommand
                {
                    Id = model.Id,
                    Urutan = model.Urutan,
                    TipeLaporan = model.TipeLaporan,
                    TipeBaris = model.TipeBaris,
                    Deskripsi = model.Deskripsi,
                    Rumus = model.Rumus,
                    Level = model.Level
                };
                await Mediator.Send(command);
            }
            else
            {
                var command = new CreateTemplateLapKeuCommand
                {
                    Urutan = model.Urutan,
                    TipeLaporan = model.TipeLaporan,
                    TipeBaris = model.TipeBaris,
                    Deskripsi = model.Deskripsi,
                    Rumus = model.Rumus,
                    Level = model.Level
                };
                await Mediator.Send(command);
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteTemplateLapKeuCommand command)
        {
            // 1. CEK DATA YANG MAU DIHAPUS DULU
            // Kita butuh tahu dia No Urut berapa & Tipe Laporannya apa
            var target = await _context.TemplateLapKeu.FindAsync(command.Id);

            if (target == null)
            {
                return Json(new { success = false, message = "Data tidak ditemukan." });
            }

            int urutanDihapus = target.Urutan;
            string tipeLaporan = target.TipeLaporan;

            // 2. HAPUS DATA TARGET
            _context.TemplateLapKeu.Remove(target);

            // 3. LOGIKA GESER NAIK (SHIFT UP)
            // Cari semua "Adik-adiknya" (Yang urutannya lebih besar)
            // Filter by TipeLaporan biar Neraca gak ngacak-ngacak Laba Rugi
            var tetanggaBawah = await _context.TemplateLapKeu
                .Where(x => x.Urutan > urutanDihapus && x.TipeLaporan == tipeLaporan)
                .ToListAsync();

            if (tetanggaBawah.Any())
            {
                foreach (var row in tetanggaBawah)
                {
                    row.Urutan -= 1; // Kurangi 1 biar naik ngisi posisi kosong
                }

                // Tandai untuk diupdate
                _context.TemplateLapKeu.UpdateRange(tetanggaBawah);
            }

            // 4. SIMPAN PERUBAHAN (Delete + Update sekaligus)
            await _context.SaveChangesAsync(System.Threading.CancellationToken.None);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Reorder([FromBody] ReorderDto model)
        {
            // 1. Ambil data yang mau dipindah
            var itemPindah = await _context.TemplateLapKeu.FindAsync(model.Id);
            if (itemPindah == null) return Json(new { success = false });

            string tipeLaporan = itemPindah.TipeLaporan;
            int urutanLama = itemPindah.Urutan;
            int urutanBaru = model.NewIndex; // Ini index dari UI (misal row ke-3 jadi urutan 3)

            // Jika posisi gak berubah, skip aja
            if (urutanLama == urutanBaru) return Json(new { success = true });

            // 2. LOGIKA GESER TETANGGA
            // Kita cari tetangga yang terdampak
            // Filter: Hanya Tipe Laporan yang sama (Biar Neraca gak geser Laba Rugi)
            
            if (urutanLama < urutanBaru) 
            {
                // SKENARIO: TURUN KE BAWAH (Misal dari 2 pindah ke 5)
                // Maka tetangga di 3, 4, 5 harus NAIK (-1)
                // Logika: Cari yang urutannya > Lama DAN <= Baru
                var tetangga = await _context.TemplateLapKeu
                    .Where(x => x.TipeLaporan == tipeLaporan && 
                                x.Urutan > urutanLama && 
                                x.Urutan <= urutanBaru)
                    .ToListAsync();

                foreach (var t in tetangga) t.Urutan -= 1;
            }
            else
            {
                // SKENARIO: NAIK KE ATAS (Misal dari 8 pindah ke 3)
                // Maka tetangga di 3, 4, 5, 6, 7 harus TURUN (+1)
                // Logika: Cari yang urutannya >= Baru DAN < Lama
                var tetangga = await _context.TemplateLapKeu
                    .Where(x => x.TipeLaporan == tipeLaporan && 
                                x.Urutan >= urutanBaru && 
                                x.Urutan < urutanLama)
                    .ToListAsync();

                foreach (var t in tetangga) t.Urutan += 1;
            }

            // 3. UPDATE ITEM UTAMA
            itemPindah.Urutan = urutanBaru;
            
            // Tandai tetangga + item utama untuk diupdate
            // _context.UpdateRange(tetangga) <-- Udah otomatis karena tracking EF
            
            await _context.SaveChangesAsync(System.Threading.CancellationToken.None);

            return Json(new { success = true });
        }

        // DTO KHUSUS UNTUK REORDER
        public class ReorderDto
        {
            public long Id { get; set; }
            public int NewIndex { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> GetTemplateOptions(string tipeLaporan)
        {
            var query = _context.TemplateLapKeu.AsQueryable();

            // 1. Filter Sesuai Tipe Laporan yang sedang diedit (NERACA/LABARUGI)
            if (!string.IsNullOrEmpty(tipeLaporan))
            {
                query = query.Where(x => x.TipeLaporan == tipeLaporan);
            }

            // 2. (Opsional) Filter supaya HEADING/SPASI gak muncul (Biar gak menuh-menuhin)
            // Kita cuma butuh DETAIL (Angka) atau TOTAL (Sub-total) untuk dijumlahkan
            query = query.Where(x => x.TipeBaris == "DETAIL" );

            var data = await query
                .OrderBy(x => x.Urutan)
                .Select(x => new 
                { 
                    Value = x.Urutan, 
                    Text = $"{x.Urutan}. {x.Deskripsi}" // Contoh: "5. Deposito"
                })
                .ToListAsync();

            return Json(data);
        }
    }
}