using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.BiayaMaterais.Commands;
using ABB.Application.BiayaMaterais.Queries;
using ABB.Application.Common.Exceptions;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.BiayaMaterai.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.BiayaMaterai
{
    public class BiayaMateraiController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetBiayaMaterais([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetBiayaMateraisQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] BiayaMateraiViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveBiayaMateraiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Data Berhasil Disimpan"});
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
        public async Task<IActionResult> Delete(string kd_mtu, decimal nilai_prm_mul, decimal nilai_prm_akh)
        {
            try
            {
                var command = new DeleteBiayaMateraiCommand()
                {
                    kd_mtu = kd_mtu, nilai_prm_mul = nilai_prm_mul, nilai_prm_akh = nilai_prm_akh,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Data Berhasil Disimpan"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public async Task<JsonResult> GetMataUang()
        {
            var result = await Mediator.Send(new GetMataUangQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return PartialView(new BiayaMateraiViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string kd_mtu, decimal nilai_prm_mul, decimal nilai_prm_akh)
        {
            var sebabKejadian = await Mediator.Send(new GetBiayaMateraiQuery()
            {
                kd_mtu = kd_mtu,
                nilai_prm_mul = nilai_prm_mul,
                nilai_prm_akh = nilai_prm_akh,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return PartialView(Mapper.Map<BiayaMateraiViewModel>(sebabKejadian));
        }
    }
}