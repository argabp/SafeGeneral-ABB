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

        public async Task<IActionResult> Add(long? id)
        {
            var dto = new TemplateLapKeuDto();

            if (id.HasValue && id.Value > 0)
            {
                // Mode Edit: Ambil data lama
                dto = await Mediator.Send(new GetTemplateLapKeuByIdQuery { Id = id.Value });
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
            await Mediator.Send(command);
            return Json(new { success = true });
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