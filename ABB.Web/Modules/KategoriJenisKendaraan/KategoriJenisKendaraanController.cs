using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Exceptions;
using ABB.Application.KategoriJenisKendaraans.Commands;
using ABB.Application.KategoriJenisKendaraans.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.KategoriJenisKendaraan.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.KategoriJenisKendaraan
{
    public class KategoriJenisKendaraanController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetKategoriJenisKendaraans([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetKategoriJenisKendaraansQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            var kategori = await Mediator.Send(new GetKategoriQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            foreach (var data in ds)
                data.nm_ref = kategori.FirstOrDefault(w => w.Value == data.kd_ref1?.Trim())?.Text;
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] KategoriJenisKendaraanViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveKategoriJenisKendaraanCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

            return PartialView("Add", model);
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(string kd_grp_rsk, string kd_rsk)
        {
            try
            {
                var command = new DeleteKategoriJenisKendaraanCommand()
                {
                    kd_grp_rsk = kd_grp_rsk, kd_rsk = kd_rsk,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public async Task<JsonResult> GetGrupResiko()
        {
            var result = await Mediator.Send(new GetGrupResikoQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKategori()
        {
            var result = await Mediator.Send(new GetKategoriQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return PartialView(new KategoriJenisKendaraanViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string kd_grp_rsk, string kd_rsk)
        {
            var kategoriJenisKendaraan = await Mediator.Send(new GetKategoriJenisKendaraanQuery()
            {
                kd_grp_rsk = kd_grp_rsk,
                kd_rsk = kd_rsk,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            kategoriJenisKendaraan.kd_grp_rsk = kategoriJenisKendaraan.kd_grp_rsk.Trim();
            kategoriJenisKendaraan.kd_ref1 = kategoriJenisKendaraan.kd_ref1?.Trim() ?? string.Empty;

            return PartialView(Mapper.Map<KategoriJenisKendaraanViewModel>(kategoriJenisKendaraan));
        }
    }
}