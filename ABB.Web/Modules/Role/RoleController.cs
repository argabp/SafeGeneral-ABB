using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Services;
using ABB.Application.Roles.Commends;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.Shared.Components.InputTags;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using ABB.Application.Roles.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Role.Models;

namespace ABB.Web.Modules.Role
{
    public class RoleController : AuthorizedBaseController
    {
        private readonly ICurrentUserService _currentUserService;

        public RoleController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            
            return View();
        }

        public async Task<JsonResult> GetRoles([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var result = await Mediator.Send(new GetRolesQuery() { SearchKeyword = searchkeyword });
            return Json(result.ToDataSourceResult(request));
        }

        private async Task<List<InputTagSourceItem>> GetUser()
        {
            var result = new List<InputTagSourceItem>();
            var users = await Mediator.Send(new GetUsersQuery());
            foreach (var user in users)
                result.Add(new InputTagSourceItem()
                {
                    Value = user.UserId,
                    Description = user.FullName,
                    ImageUrl = user.Photo
                });
            return result;
        }

        public async Task<IActionResult> Add()
        {
            var model = new AddRoleModel();
            model.RoleCode = (await Mediator.Send(new GetMaxRoleCodeQuery())) + 1;
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([FromBody] AddRoleModel model)
        {
            try
            {
                model.UserId = _currentUserService.UserId;
                await Mediator.Send(Mapper.Map<AddRoleCommand>(model));
                return Json(new { Result = "OK", Message = "Successfully Add Role" });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }

            return PartialView(model);
        }

        public async Task<IActionResult> Edit(string id = null)
        {
            var query = await Mediator.Send(new GetRoleQuery() { Id = id });
            var model = Mapper.Map<EditRoleModel>(query);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromBody] EditRoleModel model)
        {
            try
            {
                model.UserId = _currentUserService.UserId;
                await Mediator.Send(Mapper.Map<EditRoleCommand>(model));
                return Json(new { Result = "OK", Message = "Successfully Edit Role" });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await Mediator.Send(new DeleteRoleCommand() { Id = id });

            return Json(new { Result = deleted, Message = "Successfully Delete Role" });
        }
    }
}