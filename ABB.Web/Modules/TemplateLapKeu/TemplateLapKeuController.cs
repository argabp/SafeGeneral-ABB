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
        public async Task<ActionResult> GetGridData([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            // Kirim searchKeyword ke Query Handler
            var data = await Mediator.Send(new GetAllTemplateLapKeuQuery { SearchKeyword = searchKeyword }); 
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
           if (model.Urutan == 0)
            {
                // PERBAIKAN: Gunakan (int?) bukan (Urutan?)
                // Tanda tanya (?) penting supaya kalau tabel kosong, dia return null (bukan error)
                // ?? 0 artinya kalau hasil null (tabel kosong), anggap 0.
                var maxUrutan = await _context.TemplateLapKeu.MaxAsync(x => (int?)x.Urutan) ?? 0;
                
                // Data baru akan ditaruh di paling bawah
                model.Urutan = maxUrutan + 1;
            }

            if (model.Id > 0)
            {
                var command = new UpdateTemplateLapKeuCommand
                {
                    Id = model.Id,
                    Urutan = model.Urutan, // Pastikan ini dikirim
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
                    Urutan = model.Urutan, // Pastikan ini dikirim
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
    }
}