using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.BiayaMaterais.Queries;
using ABB.Application.Common.Dtos;
using ABB.Application.EntriNotas.Queries;
using ABB.Application.NotaKomisiTambahans.Commands;
using ABB.Application.NotaKomisiTambahans.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.NotaKomisiTambahan.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using GetRekananTertujuQuery = ABB.Application.NotaKomisiTambahans.Queries.GetRekananTertujuQuery;

namespace ABB.Web.Modules.NotaKomisiTambahan
{
    public class NotaKomisiTambahanController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return PartialView(new NotaKomisiTambahanViewModel()
            {
                jns_sb_nt = "U",
                kd_cb = string.Empty,
                jns_tr = "P",
                jns_nt_msk = "0",
                kd_thn = string.Empty,
                kd_bln = string.Empty,
                no_nt_msk = string.Empty,
                jns_nt_kel = string.Empty,
                no_nt_kel = "00",
                kd_mtu = "001",
                kd_grp_ttj = "9",
                uraian = "KOMISI",
            });
        }

        [HttpGet]
        public IActionResult NomorAkseptasi()
        {
            return PartialView();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string jns_sb_nt, string kd_cb, string jns_tr, string jns_nt_msk, 
            string kd_thn, string kd_bln, string no_nt_msk, string jns_nt_kel, string no_nt_kel)
        {
            var command = new GetNotaKomisiTambahanQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                jns_sb_nt = jns_sb_nt,
                kd_cb = kd_cb,
                jns_tr = jns_tr,
                jns_nt_msk = jns_nt_msk,
                kd_thn = kd_thn,
                kd_bln = kd_bln,
                no_nt_msk = no_nt_msk,
                jns_nt_kel = jns_nt_kel,
                no_nt_kel = no_nt_kel,
            };
            
            var data = await Mediator.Send(command);
            
            return PartialView(Mapper.Map<NotaKomisiTambahanViewModel>(data));
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(string jns_sb_nt, string kd_cb, string jns_tr, string jns_nt_msk, 
            string kd_thn, string kd_bln, string no_nt_msk, string jns_nt_kel, string no_nt_kel)
        {
            try
            {
                var command = new DeleteNotaKomisiTambahanCommand()
                {
                    jns_sb_nt = jns_sb_nt,
                    kd_cb = kd_cb,
                    jns_tr = jns_tr,
                    jns_nt_msk = jns_nt_msk,
                    kd_thn = kd_thn,
                    kd_bln = kd_bln,
                    no_nt_msk = no_nt_msk,
                    jns_nt_kel = jns_nt_kel,
                    no_nt_kel = no_nt_kel,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Nota Tambahan"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public async Task<ActionResult> GetNotaKomisiTambahans([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetNotaKomisiTambahansQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetNomorAkseptasis([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetNomorAkseptasiQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveNotaKomisiTambahan([FromBody] NotaKomisiTambahanViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveNotaKomisiTambahanCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Save Nota Komisi Tambahan"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        public async Task<JsonResult> GetMataUang()
        {
            var result = await Mediator.Send(new GetMataUangQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeTertuju()
        {
            var result = await Mediator.Send(new GetKodeTertujuQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetNomorPolis()
        {
            var result = await Mediator.Send(new GetNomorAkseptasiQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public JsonResult GetJenisNota()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "", Value = "" },
                new DropdownOptionDto() { Text = "Komisi", Value = "C" },
                new DropdownOptionDto() { Text = "Diskon", Value = "D" }
            };

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeRekananTertuju(string kd_grp_rk)
        {
            var result = await Mediator.Send(new GetRekananTertujuQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_grp_rk = kd_grp_rk
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetTertanggungData(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_pol, Int16 no_updt, string kd_mtu)
        {
            var result = await Mediator.Send(new GetTertanggungDataQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                kd_mtu_1 = kd_mtu,
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetNilaiPremiAndPercentageNt(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_pol, Int16 no_updt, string kd_mtu, decimal nilai_nt)
        {
            var result = await Mediator.Send(new GetNilaiPremiAndPercentageNtQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                kd_mtu = kd_mtu,
                nilai_nt = nilai_nt,
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetPercentage(string jns_nt_kel, string kd_grp_ttj,
            decimal nilai_nt, string kd_rk_ttj)
        {
            var result = await Mediator.Send(new GetPercentageQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                jns_nt_kel = jns_nt_kel,
                kd_grp_ttj = kd_grp_ttj,
                nilai_nt = nilai_nt,
                kd_rk_ttj = kd_rk_ttj
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetPstShareAndNilaiPremiReas(string kd_cb,
            string kd_grp_ttj, string kd_rk_ttj)
        {
            var result = await Mediator.Send(new GetPstShareAndNilaiPremiReasQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cb = kd_cb,
                kd_grp_ttj = kd_grp_ttj,
                kd_rk_ttj = kd_rk_ttj
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetNilaiAndPercentage(string kd_mtu, DateTime tgl_nt,
            decimal pst_nt, decimal nilai_nt, string jns_nt_kel, string kd_grp_ttj,
            string uraian, string kd_cb, string kd_rk_ttj)
        {
            var result = await Mediator.Send(new GetNilaiAndPercentageQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_mtu = kd_mtu,
                tgl_nt = tgl_nt,
                pst_nt = pst_nt,
                nilai_nt = nilai_nt,
                jns_nt_kel = jns_nt_kel,
                kd_grp_ttj = kd_grp_ttj,
                uraian = uraian,
                kd_cb = kd_cb,
                kd_rk_ttj = kd_rk_ttj
            });

            return Json(result);
        }
    }
}