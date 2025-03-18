using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.TarifPerluasanKendaraanOJKs.Commands;
using ABB.Application.TarifPerluasanKendaraanOJKs.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.TarifPerluasanKendaraanOJK.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.TarifPerluasanKendaraanOJK
{
    public class TarifPerluasanKendaraanOJKController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            
            return View();
        }
        
        public async Task<ActionResult> GetKategoris([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetKategorisQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetTarifPerluasanKendaraanOJKs([DataSourceRequest] DataSourceRequest request, string kd_kategori)
        {
            if (string.IsNullOrWhiteSpace(kd_kategori))
                return Ok();

            var ds = await Mediator.Send(new GetTarifPerluasanKendaraanOJKsQuery()
            {
                kd_kategori = kd_kategori,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            foreach (var data in ds)
                data.nm_stn_rate_prm = data.stn_rate_prm == 1 ? "%" : "%o";
            
            var wilayah = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Wilayah I", Value = "1" },
                new DropdownOptionDto() { Text = "Wilayah II", Value = "2" },
                new DropdownOptionDto() { Text = "Wilayah III", Value = "3" }
            };

            foreach (var data in ds)
                data.nm_wilayah = wilayah.FirstOrDefault(w => w.Value == data.kd_wilayah?.Trim())?.Text;
            
            var bentukPertanggungan = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "TLO", Value = "D" },
                new DropdownOptionDto() { Text = "All Risk", Value = "E" },
                new DropdownOptionDto() { Text = "All Risk / TLO", Value = "9" }
            };

            foreach (var data in ds)
                data.nm_jns_ptg = bentukPertanggungan.FirstOrDefault(w => w.Value == data.kd_jns_ptg?.Trim())?.Text;

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        public JsonResult GetWilayah()
        {
            var wilayah = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Wilayah I", Value = "1" },
                new DropdownOptionDto() { Text = "Wilayah II", Value = "2" },
                new DropdownOptionDto() { Text = "Wilayah III", Value = "3" }
            };

            return Json(wilayah);
        }
        
        public JsonResult GetBentukPertanggungan()
        {
            var bentukPertanggungan = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "TLO", Value = "D" },
                new DropdownOptionDto() { Text = "All Risk", Value = "E" },
                new DropdownOptionDto() { Text = "All Risk / TLO", Value = "9" }
            };

            return Json(bentukPertanggungan);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] TarifPerluasanKendaraanOJKViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveTarifPerluasanKendaraanOJKCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Save Tarif Perluasan Kendaraan OJK"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(string kd_kategori, string kd_jns_ptg, string kd_wilayah, byte no_kategori)
        {
            try
            {
                await Mediator.Send(new DeleteTarifPerluasanKendaraanOJKCommand()
                {
                    kd_kategori = kd_kategori,
                    kd_jns_ptg = kd_jns_ptg,
                    kd_wilayah = kd_wilayah,
                    no_kategori = no_kategori,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                });
                return Json(new { Result = "OK", Message = "Successfully Delete Tarif Kendaraan OJK"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        public JsonResult GetSatuanRate()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "%", Value = "1" },
                new DropdownOptionDto() { Text = "%o", Value = "10" }
            };

            return Json(result);
        }

        [HttpGet]
        public IActionResult Add(string kd_kategori)
        {
            return PartialView(new TarifPerluasanKendaraanOJKViewModel()
            {
                kd_kategori = kd_kategori
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string kd_kategori, string kd_jns_ptg, string kd_wilayah, byte no_kategori)
        {
            var tarifPerluasanKendaraanOjk = await Mediator.Send(new GetTarifPerluasanKendaraanOJKQuery()
            {
                kd_kategori = kd_kategori,
                kd_jns_ptg = kd_jns_ptg,
                kd_wilayah = kd_wilayah,
                no_kategori = no_kategori,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            tarifPerluasanKendaraanOjk.kd_kategori = tarifPerluasanKendaraanOjk.kd_kategori.Trim();
            tarifPerluasanKendaraanOjk.kd_jns_ptg = tarifPerluasanKendaraanOjk.kd_jns_ptg.Trim();
            tarifPerluasanKendaraanOjk.kd_wilayah = tarifPerluasanKendaraanOjk.kd_wilayah.Trim();
            
            return PartialView(Mapper.Map<TarifPerluasanKendaraanOJKViewModel>(tarifPerluasanKendaraanOjk));
        }
    }
}