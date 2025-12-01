using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.ReopenMutasiKlaims.Commands;
using ABB.Application.ReopenMutasiKlaims.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.ReopenMutasiKlaim.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.ReopenMutasiKlaim
{
    public class ReopenMutasiKlaimController : AuthorizedBaseController
    {
        private static List<DropdownOptionDto> _tipeMutasi;
        
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            _tipeMutasi = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "PLA", Value = "P" },
                new DropdownOptionDto() { Text = "DLA", Value = "D" },
                new DropdownOptionDto() { Text = "Beban", Value = "B" },
                new DropdownOptionDto() { Text = "Recovery", Value = "R" }
            };
            
            return View();
        }
        
        public async Task<ActionResult> GetReopenMutasiKlaims([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetReopenMutasiKlaimQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });
            
            var counter = 1;
            foreach (var data in ds)
            {
                data.Id = counter;
                data.nm_tipe_mts = _tipeMutasi.FirstOrDefault(w => w.Value.Trim() == data.tipe_mts.Trim())?.Text ??
                                   string.Empty;
                data.nomor_register = "K." + data.kd_cb.Trim() + "." + data.kd_scob.Trim() 
                                      + "." + data.kd_thn.Trim() + "." + data.no_kl.Trim();
                
                counter++;
            }

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<ActionResult> ReopenMutasiKlaim([FromBody] ReopenMutasiKlaimViewModel model)
        {
            try
            {
                var command = Mapper.Map<ReopenMutasiKlaimCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
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