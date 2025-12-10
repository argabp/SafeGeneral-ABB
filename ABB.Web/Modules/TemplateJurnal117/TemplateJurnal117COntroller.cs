using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.TemplateJurnals117.Commands;
using ABB.Application.TemplateJurnals117.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.TemplateJurnal117.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.TemplateJurnal117
{
    public class TemplateJurnal117Controller : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetTemplateJurnal117([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetAllTemplateJurnal117Query()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

       public async Task<ActionResult> GetDetailTemplateJurnal117(
    [DataSourceRequest] DataSourceRequest request, 
    string Type, 
    string JenisAss)
            {
                var trimmedType = Type?.Trim();
                var trimmedJenisAss = JenisAss?.Trim();
                
                if (string.IsNullOrWhiteSpace(trimmedType) || string.IsNullOrWhiteSpace(trimmedJenisAss))
                    return Ok();

                var ds = await Mediator.Send(new GetTemplateJurnalDetail117Query
                {
                    Type = trimmedType,
                    JenisAss = trimmedJenisAss,
                    // TAMBAHKAN INI agar connect ke DB yang benar
                    DatabaseName = Request.Cookies["DatabaseValue"] 
                });

                return Json(ds.AsQueryable().ToDataSourceResult(request));
            }

        [HttpPost]
        public async Task<IActionResult> DeleteTemplateJurnal117([FromBody] DeleteTemplateJurnal117ViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteTemplateJurnal117Command>(model);
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
        public async Task<IActionResult> AddTemplateJurnal117([FromBody] SaveTemplateJurnal117ViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddTemplateJurnal117Command>(model);
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
        public async Task<IActionResult> EditTemplateJurnal117([FromBody] SaveTemplateJurnal117ViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditTemplateJurnal117Command>(model);
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
        public async Task<IActionResult> AddTemplateJurnalDetail117([FromBody] SaveTemplateJurnalDetail117ViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddTemplateJurnalDetail117Command>(model);
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
            public async Task<IActionResult> EditTemplateJurnalDetail117([FromBody] SaveTemplateJurnalDetail117ViewModel model)
            {
                try
                {
                    // Mapping
                    var command = Mapper.Map<EditTemplateJurnalDetail117Command>(model);
                    
                    // PERBAIKAN: Isi DatabaseName manual karena tidak ada di body JSON
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
        public async Task<IActionResult> DeleteTemplateJurnalDetail117([FromBody] DeleteTemplateJurnalDetail117ViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteTemplateJurnalDetail117Command>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        
    }
}