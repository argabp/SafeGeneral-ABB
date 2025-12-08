using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.TemplateJurnals62.Commands;
using ABB.Application.TemplateJurnals62.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.TemplateJurnal62.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.TemplateJurnal62
{
    public class TemplateJurnal62Controller : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetTemplateJurnal62([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetAllTemplateJurnal62Query()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

       public async Task<ActionResult> GetDetailTemplateJurnal62(
    [DataSourceRequest] DataSourceRequest request, 
    string Type, 
    string JenisAss)
            {
                var trimmedType = Type?.Trim();
                var trimmedJenisAss = JenisAss?.Trim();
                
                if (string.IsNullOrWhiteSpace(trimmedType) || string.IsNullOrWhiteSpace(trimmedJenisAss))
                    return Ok();

                var ds = await Mediator.Send(new GetTemplateJurnalDetail62Query
                {
                    Type = trimmedType,
                    JenisAss = trimmedJenisAss,
                    // TAMBAHKAN INI agar connect ke DB yang benar
                    DatabaseName = Request.Cookies["DatabaseValue"] 
                });

                return Json(ds.AsQueryable().ToDataSourceResult(request));
            }

        [HttpPost]
        public async Task<IActionResult> DeleteTemplateJurnal62([FromBody] DeleteTemplateJurnal62ViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteTemplateJurnal62Command>(model);
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
        public async Task<IActionResult> AddTemplateJurnal62([FromBody] SaveTemplateJurnal62ViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddTemplateJurnal62Command>(model);
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
        public async Task<IActionResult> EditTemplateJurnal62([FromBody] SaveTemplateJurnal62ViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditTemplateJurnal62Command>(model);
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
        public async Task<IActionResult> AddTemplateJurnalDetail62([FromBody] SaveTemplateJurnalDetail62ViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddTemplateJurnalDetail62Command>(model);
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
            public async Task<IActionResult> EditTemplateJurnalDetail62([FromBody] SaveTemplateJurnalDetail62ViewModel model)
            {
                try
                {
                    // Mapping
                    var command = Mapper.Map<EditTemplateJurnalDetail62Command>(model);
                    
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
        public async Task<IActionResult> DeleteTemplateJurnalDetail62([FromBody] DeleteTemplateJurnalDetail62ViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteTemplateJurnalDetail62Command>(model);
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