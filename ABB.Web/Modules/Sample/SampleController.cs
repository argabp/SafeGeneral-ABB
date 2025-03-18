using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Roles.Commends;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.Sample.Models;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ABB.Web.Modules.Sample
{
    public class SampleController : BaseController
    {
        public IActionResult Report()
        {
            return View();
        }

        public IActionResult ReportDesigner()
        {
            return View();
        }

        public IActionResult Index()
        {
            // var model = new SampleViewModels();
            // model.Products = await Mediator.Send(new GetProductsQuery());
            // model.ProductTypes = await Mediator.Send(new GetProductTypeQuery());
            // var interactionGroups = new List<InteractionGroupModel>();
            // interactionGroups.Add(new InteractionGroupModel() { Index = 0, InteractionGroupId = 10, InteractionGroupName = "Not Connected" });
            // interactionGroups.Add(new InteractionGroupModel() { Index = 1, InteractionGroupId = 20, InteractionGroupName = "Not Contacted" });
            // model.InteractionGroups =  interactionGroups;
            return View();
        }
        
        public IActionResult ViewReport()
        {
            var model = JsonConvert.SerializeObject(null);
            
            return PartialView("ViewReport", model);
        }
    }
}