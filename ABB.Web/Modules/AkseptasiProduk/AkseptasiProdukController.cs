using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.AkseptasiProduks.Commands;
using ABB.Application.AkseptasiProduks.Queries;
using ABB.Application.Common;
using ABB.Application.PolisInduks.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Modules.AkseptasiProduk.Models;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.AkseptasiProduk
{
    public class AkseptasiProdukController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetAkseptasiProduks([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetAkseptasiProduksQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public IActionResult Add()
        {
            return View(new AkseptasiProdukViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AkseptasiProdukViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddAkseptasiProdukCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        public async Task<IActionResult> Edit(string kd_cob, string kd_scob)
        {
            var akseptasiProduk = await Mediator.Send(new GetAkseptasiProdukQuery()
            {
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            akseptasiProduk.kd_cob = kd_cob.Trim();
            akseptasiProduk.kd_scob = kd_scob.Trim();
            
            return View(Mapper.Map<AkseptasiProdukViewModel>(akseptasiProduk));
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] AkseptasiProdukViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditAkseptasiProdukCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        public async Task<IActionResult> Delete(string kd_cob, string kd_scob)
        {
            try
            {
                await Mediator.Send(new DeleteAkseptasiProdukCommand()
                {
                    DatabaseName = Request.Cookies["DatabaseValue"],
                    kd_cob = kd_cob,
                    kd_scob = kd_scob
                });
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        public async Task<JsonResult> GetCOB()
        {
            var result = await Mediator.Send(new GetCobQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }

        public async Task<JsonResult> GetSCOB(string kd_cob)
        {
            var result = await Mediator.Send(new GetSCOBQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cob = kd_cob
            });

            return Json(result);
        }
    }
}