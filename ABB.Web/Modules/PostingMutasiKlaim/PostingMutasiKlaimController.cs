using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.PostingMutasiKlaims.Commands;
using ABB.Application.PostingMutasiKlaims.Queries;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.PostingMutasiKlaim
{
    public class PostingMutasiKlaimController : AuthorizedBaseController
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
        
        public async Task<ActionResult> GetPostingMutasiKlaims([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetPostingMutasiKlaimQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
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
        public async Task<ActionResult> Posting([FromBody] List<PostingMutasiKlaimModel> model)
        {
            try
            {
                var command = new PostingMutasiKlaimCommand()
                {
                    DatabaseName = Request.Cookies["DatabaseValue"],
                    Data = model
                };

                await Mediator.Send(command);

                return Ok(new { Status = "OK"});
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
    }
}