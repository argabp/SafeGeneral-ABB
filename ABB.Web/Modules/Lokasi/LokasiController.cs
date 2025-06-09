using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Exceptions;
using ABB.Application.Lokasis.Commands;
using ABB.Application.Lokasis.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.Lokasi.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Lokasi
{
    public class LokasiController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetProvinsi([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetProvinsiQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetKabupaten([DataSourceRequest] DataSourceRequest request, string kd_prop)
        {
            if (string.IsNullOrWhiteSpace(kd_prop))
                return Ok();
            
            var ds = await Mediator.Send(new GetKabupatenQuery()
            {
                kd_prop = kd_prop,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetKecamatan([DataSourceRequest] DataSourceRequest request, string kd_prop, string kd_kab)
        {
            if (string.IsNullOrWhiteSpace(kd_prop) || string.IsNullOrWhiteSpace(kd_kab))
                return Ok();
            
            var ds = await Mediator.Send(new GetKecamatanQuery() 
            { 
                kd_prop = kd_prop,
                kd_kab = kd_kab,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetKelurahan([DataSourceRequest] DataSourceRequest request, string kd_prop, 
            string kd_kab, string kd_kec)
        {
            if (string.IsNullOrWhiteSpace(kd_prop) || string.IsNullOrWhiteSpace(kd_kab) || string.IsNullOrWhiteSpace(kd_kec))
                return Ok();
            
            var ds = await Mediator.Send(new GetKelurahanQuery() { 
                kd_prop = kd_prop,
                kd_kab = kd_kab,
                kd_kec = kd_kec,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveProvinsi([FromBody] ProvinsiViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveProvinsiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteProvinsi([FromBody] ProvinsiViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteProvinsiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveKabupaten([FromBody] KabupatenViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveKabupatenCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Errors });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteKabupaten([FromBody] KabupatenViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteKabupatenCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveKecamatan([FromBody] KecamatanViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveKecamatanCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Errors });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteKecamatan([FromBody] KecamatanViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteKecamatanCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveKelurahan([FromBody] KelurahanViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveKelurahanCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Errors });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteKelurahan([FromBody] KelurahanViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteKelurahanCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public async Task<ActionResult> GetLokasiResiko([DataSourceRequest] DataSourceRequest request)
        {
            var ds = await Mediator.Send(new GetLokasiResikoQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
        return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveLokasiResiko([FromBody] LokasiResikoViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveLokasiResikoCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteLokasiResiko([FromBody] LokasiResikoViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteLokasiResikoCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        public async Task<ActionResult> GetDetailLokasiResiko([DataSourceRequest] DataSourceRequest request, string kd_pos)
        {
            var ds = await Mediator.Send(new GetDetailLokasiResikoQuery()
            {
                kd_pos = kd_pos,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveDetailLokasiResiko([FromBody] DetailLokasiResikoViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDetailLokasiResikoCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteDetailLokasiResiko([FromBody] DetailLokasiResikoViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteDetailLokasiResikoCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProvinsiDropdown()
        {
            var provinsi = await Mediator.Send(new GetProvinsiQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            var provinsiDropdown = new List<DropdownOptionDto>();
            
            foreach (var prov in provinsi)
            {
                provinsiDropdown.Add(new DropdownOptionDto()
                {
                    Text = prov.nm_prop,
                    Value = prov.kd_prop
                });
            }

            return Json(provinsiDropdown);
        }

        [HttpGet]
        public async Task<IActionResult> GetKabupatenDropdown(string kd_prop)
        {
            var kabupaten = await Mediator.Send(new GetKabupatenQuery()
            {
                kd_prop = kd_prop,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            var kabupatenDropdown = new List<DropdownOptionDto>();

            foreach (var kab in kabupaten)
            {
                kabupatenDropdown.Add(new DropdownOptionDto()
                {
                    Text = kab.nm_kab,
                    Value = kab.kd_kab
                });
            }

            return Json(kabupatenDropdown);
        }

        [HttpGet]
        public async Task<IActionResult> GetKecamatanDropdown(string kd_prop, string kd_kab)
        {
            var kecamatan = await Mediator.Send(new GetKecamatanQuery()
            {
                kd_prop = kd_prop, kd_kab = kd_kab,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            var kecamatanDropdown = new List<DropdownOptionDto>();

            foreach (var kab in kecamatan)
            {
                kecamatanDropdown.Add(new DropdownOptionDto()
                {
                    Text = kab.nm_kec,
                    Value = kab.kd_kec
                });
            }

            return Json(kecamatanDropdown);
        }

        [HttpGet]
        public async Task<IActionResult> GetKelurahanDropdown(string kd_prop, string kd_kab, string kd_kec)
        {
            var kelurahan = await Mediator.Send(new GetKelurahanQuery()
            {
                kd_prop = kd_prop,
                kd_kab = kd_kab,
                kd_kec = kd_kec,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            var kelurahanDropdown = new List<DropdownOptionDto>();

            foreach (var kab in kelurahan)
            {
                kelurahanDropdown.Add(new DropdownOptionDto()
                {
                    Text = kab.nm_kel,
                    Value = kab.kd_kel
                });
            }

            return Json(kelurahanDropdown);
        }

        [HttpGet]
        public IActionResult AddDetailLokasiResikoView(string kd_pos)
        {
            return PartialView(new DetailLokasiResikoViewModel() { kd_pos = kd_pos});
        }

        [HttpGet]
        public async Task<IActionResult> EditDetailLokasiResikoView(string kd_pos, string kd_lok_rsk)
        {
            var detailLokasiResiko = await Mediator.Send(new GetSingleDetailLokasiResikoQuery()
            {
                kd_lok_rsk = kd_lok_rsk,
                kd_pos = kd_pos,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return PartialView(Mapper.Map<DetailLokasiResikoViewModel>(detailLokasiResiko));
        }
    }
}