using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
//using ABB.Application.TemplateJurnals62.Commands;
using ABB.Application.TemplateJurnals62.Queries;
using ABB.Web.Modules.Base;
//using ABB.Web.Modules.TemplateJurnal62.Models;
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
            if (string.IsNullOrWhiteSpace(Type) || string.IsNullOrWhiteSpace(JenisAss))
                return Ok();

            var ds = await Mediator.Send(new GetTemplateJurnalDetail62Query
            {
                Type = Type,
                JenisAss = JenisAss,
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
    }
}