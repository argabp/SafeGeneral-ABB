using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Approvals.Commands;
using ABB.Application.Approvals.Queries;
using ABB.Application.Common;
using ABB.Application.DokumenAkseptasis.Commands;
using ABB.Application.DokumenAkseptasis.Queries;
using ABB.Application.PengajuanAkseptasi.Queries;
using ABB.Application.PolisInduks.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Modules.Approval.Models;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.DokumenAkseptasi.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.DokumenAkseptasi
{
    public class DokumenAkseptasiController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetDokumenAkseptasis([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetDokumenAkseptasisQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetDokumenAkseptasiDetails([DataSourceRequest] DataSourceRequest request, 
            string kd_cob, string kd_scob)
        {
            var ds = await Mediator.Send(new GetDokumenAkseptasiDetilsQuery()
            {
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public IActionResult Add()
        {
            return View(new DokumenAkseptasiViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] DokumenAkseptasiViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddDokumenAkseptasiCommand>(model);
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
            var dokumenAkseptasi = await Mediator.Send(new GetDokumenAkseptasiQuery()
            {
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            dokumenAkseptasi.kd_cob = kd_cob.Trim();
            dokumenAkseptasi.kd_scob = kd_scob.Trim();
            
            return View(Mapper.Map<DokumenAkseptasiViewModel>(dokumenAkseptasi));
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] DokumenAkseptasiViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditDokumenAkseptasiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteDokumenAkseptasiViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteDokumenAkseptasiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        public IActionResult AddDetail(string kd_cob, string kd_scob)
        {
            return View(new DokumenAkseptasiDetailViewModel()
            {
                kd_cob = kd_cob,
                kd_scob = kd_scob
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddDetail([FromBody] DokumenAkseptasiDetailViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddDokumenAkseptasiDetilCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        
        public async Task<IActionResult> EditDetail(string kd_cob, string kd_scob, Int16 kd_dokumen)
        {
            var detail = await Mediator.Send(new GetDokumenAkseptasiDetilQuery()
            {
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_dokumen = kd_dokumen,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            detail.kd_cob = kd_cob.Trim();
            detail.kd_scob = kd_scob.Trim();
            
            return View(Mapper.Map<DokumenAkseptasiDetailViewModel>(detail));
        }
        
        [HttpPost]
        public async Task<IActionResult> EditDetail([FromBody] DokumenAkseptasiDetailViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditDokumenAkseptasiDetilCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteDetail([FromBody] DeleteDokumenAkseptasiDetailViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteDokumenAkseptasiDetilCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
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
        
        public async Task<JsonResult> GetKodeDokumen()
        {
            var command = new GetDokumensQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            };
            
            var result = await Mediator.Send(command);

            return Json(result);
        }
    }
}