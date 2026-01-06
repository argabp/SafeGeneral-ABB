using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Queries;
using ABB.Application.DokumenKlaims.Commands;
using ABB.Application.DokumenKlaims.Queries;
using ABB.Application.PengajuanAkseptasi.Queries;
using ABB.Application.PolisInduks.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.DokumenKlaim.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.DokumenKlaim
{
    public class DokumenKlaimController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetDokumenKlaims([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetDokumenKlaimsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetDokumenKlaimDetails([DataSourceRequest] DataSourceRequest request, 
            string kd_cob, string kd_scob)
        {
            var ds = await Mediator.Send(new GetDokumenKlaimDetilsQuery()
            {
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public IActionResult Add()
        {
            return View(new DokumenKlaimViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] DokumenKlaimViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDokumenKlaimCommand>(model);
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
            var dokumenKlaim = await Mediator.Send(new GetDokumenKlaimQuery()
            {
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            dokumenKlaim.kd_cob = kd_cob.Trim();
            dokumenKlaim.kd_scob = kd_scob.Trim();
            
            return View(Mapper.Map<DokumenKlaimViewModel>(dokumenKlaim));
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteDokumenKlaimViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteDokumenKlaimCommand>(model);
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
            return View(new DokumenKlaimDetailViewModel()
            {
                kd_cob = kd_cob,
                kd_scob = kd_scob
            });
        }

        [HttpPost]
        public async Task<IActionResult> SaveDetail([FromBody] DokumenKlaimDetailViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDokumenKlaimDetilCommand>(model);
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
            var detail = await Mediator.Send(new GetDokumenKlaimDetilQuery()
            {
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_dokumen = kd_dokumen,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            detail.kd_cob = kd_cob.Trim();
            detail.kd_scob = kd_scob.Trim();
            
            return View(Mapper.Map<DokumenKlaimDetailViewModel>(detail));
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteDetail([FromBody] DeleteDokumenKlaimDetailViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteDokumenKlaimDetilCommand>(model);
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