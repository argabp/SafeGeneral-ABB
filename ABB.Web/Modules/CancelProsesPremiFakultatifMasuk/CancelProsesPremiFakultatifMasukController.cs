using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.CancelProsesPremiFakultatifMasuks.Queries;
using ABB.Application.ReopenPolis.Commands;
using ABB.Application.ReopenPolis.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.ReopenPolis.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CancelProsesPremiFakultatifMasuk
{
  public class CancelProsesPremiFakultatifMasukController : AuthorizedBaseController
  {
    private const string DatabaseName = "abb_kp00";
    private const string KodeCabang = "PS10";
    
    public ActionResult Index()
    {
      ViewBag.Module = Request.Cookies["Module"];
      ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
      ViewBag.UserLogin = CurrentUser.UserId;
            
      return View();
    }
        
    public async Task<ActionResult> GetCancelProsesPremiFakultatifMasuks([DataSourceRequest] DataSourceRequest request, string searchkeyword)
    {
      var ds = await Mediator.Send(new GetCancelProsesPremiFakultatifMasuksQuery()
      {
        SearchKeyword = searchkeyword,
        DatabaseName = DatabaseName,
        KodeCabang = KodeCabang
      });

      return Json(ds.AsQueryable().ToDataSourceResult(request));
    }
        
    [HttpPost]
    public async Task<ActionResult> Cancel([FromBody] ReopenPolisViewModel model)
    {
      try
      {
        var command = Mapper.Map<ReopenPolisCommand>(model);
        command.DatabaseName = DatabaseName;
        await Mediator.Send(command);

        return Ok(new { Status = "OK" });
      }
      catch (Exception e)
      {
        return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
      }
    }
  }
}