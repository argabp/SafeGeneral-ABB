using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Exceptions;
using ABB.Application.EmailTemplates.Commands;
using ABB.Application.EmailTemplates.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.EmailTemplate.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.EmailTemplate
{
    public class EmailTemplateController : AuthorizedBaseController
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.UserLogin = CurrentUser.UserId;
            ViewBag.RoleLogin = await CurrentUser.GetRoleName();
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetEmailTemplates([DataSourceRequest] DataSourceRequest request)
        {
            var ds = await Mediator.Send(new GetEmailTemplatesQuery());
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpGet]
        public IActionResult Add()
        {
            var model = new EmailTemplateViewModel();

            return PartialView(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddEmailTemplate([FromBody] EmailTemplateViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddEmailTemplateCommand>(model);
                await Mediator.Send(command);

                return Json(new { Result = "OK", Message = Constant.DataDisimpan });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception e)
            {
                return Json(new
                    { Result = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
            }

            return PartialView("Add", model);
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var query = await Mediator.Send(new GetEmailTemplateQuery() { Id = id });

            var model = Mapper.Map<EmailTemplateViewModel>(query);
            
            return PartialView(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> EditEmailTemplate([FromBody] EmailTemplateViewModel model)
        {
            try
            {
                var commnad = Mapper.Map<EditEmailTemplateCommand>(model);
                await Mediator.Send(commnad);
                
                return Json(new { Result = "OK", Message = Constant.DataDisimpan });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception e)
            {
                return Json(new
                    { Result = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
            }
            
            return PartialView("Edit", model);
        }
        
        [HttpGet]
        public async Task<ActionResult> DeleteEmailTemplate(int id)
        {
            try
            {
                await Mediator.Send(new DeleteEmailTemplateCommand() { Id = id });
                
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception e)
            {
                return Json(new
                    { Result = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
            }
        }
    }
}