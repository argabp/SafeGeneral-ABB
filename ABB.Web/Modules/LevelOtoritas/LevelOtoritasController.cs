using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Akuisisis.Commands;
using ABB.Application.Akuisisis.Queries;
using ABB.Application.BiayaPerSubCOBs.Queries;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Exceptions;
using ABB.Application.LevelOtoritass.Commands;
using ABB.Application.LevelOtoritass.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Akuisisi.Models;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.LevelOtoritas.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.LevelOtoritas
{
    public class LevelOtoritasController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetLevelOtoritass([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetLevelOtoritassQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            var flag_xol = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "EX-gratia", Value = "E" },
                new DropdownOptionDto() { Text = "Posting", Value = "P" },
                new DropdownOptionDto() { Text = "XOL", Value = "X" }
            };
            
            foreach (var data in ds)
                data.flag_xol = flag_xol.FirstOrDefault(w => w.Value == data.flag_xol)?.Text;

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] LevelOtoritasViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveLevelOtoritasCommand>(model);
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
        public async Task<IActionResult> Delete(string kd_user)
        {
            try
            {
                var command = new DeleteLevelOtoritasCommand()
                {
                    kd_user = kd_user,
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

        public JsonResult GetLevelOtoritas()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "EX-gratia", Value = "E" },
                new DropdownOptionDto() { Text = "Posting", Value = "P" },
                new DropdownOptionDto() { Text = "XOL", Value = "X" }
            };

            return Json(result);
        }
        
        [HttpGet]
        public IActionResult Add()
        {
            return PartialView(new LevelOtoritasViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string kd_user)
        {
            var levelOtoritas = await Mediator.Send(new GetLevelOtoritasQuery()
            {
                kd_user = kd_user,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return PartialView(Mapper.Map<LevelOtoritasViewModel>(levelOtoritas));
        }
    }
}