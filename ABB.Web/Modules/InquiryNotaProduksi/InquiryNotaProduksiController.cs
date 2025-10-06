using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.InquiryNotaProduksis.Queries;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using ABB.Web.Modules.InquiryNotaProduksi.Models;

namespace ABB.Web.Modules.InquiryNotaProduksi
{
    public class InquiryNotaProduksiController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetInquiryNotaProduksi(
            [DataSourceRequest] DataSourceRequest request, 
            string searchKeyword,
            DateTime? startDate,
            DateTime? endDate)
        {
            var data = await Mediator.Send(new InquiryNotaProduksiQuery() 
            { 
                SearchKeyword = searchKeyword,
                StartDate = startDate,
                EndDate = endDate
            });

            return Json(await data.ToDataSourceResultAsync(request));
        }

        public async Task<IActionResult> Add(int id)
        {

            var databaseName = Request.Cookies["DatabaseName"];

            var InquiryNotaProduksiDto = await Mediator.Send(new GetInquiryNotaProduksiByIdQuery { id = id });
            if (InquiryNotaProduksiDto == null) return NotFound();

            var viewModel = new InquiryNotaProduksiViewModel
            {
                InquiryNotaProduksiHeader = InquiryNotaProduksiDto,
                id = id // Langsung set di properti utama
            };

            return PartialView(viewModel);
        }
    }
}
