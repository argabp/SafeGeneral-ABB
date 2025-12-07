using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.SlideShows.Commands;
using ABB.Application.SlideShows.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.SlideShow.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.SlideShow
{
    public class SlideShowController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetSlideShows([DataSourceRequest] DataSourceRequest request)
        {
            var ds = await Mediator.Send(new GetSlideShowsQuery());
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveSlideShow(SlideShowViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveSlideShowCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        public async Task<IActionResult> DeleteSlideShow(int Id)
        {
            try
            {
                var command = new DeleteSlideShowCommand()
                {
                    Id = Id
                };
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