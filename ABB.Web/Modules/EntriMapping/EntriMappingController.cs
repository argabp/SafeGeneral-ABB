using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ABB.Application.EntriMappings.Commands;
using ABB.Application.EntriMappings.Queries;
using ABB.Web.Modules.EntriMapping.Models;
using ABB.Application.Coas.Queries;
using ABB.Application.Coas117.Queries;

namespace ABB.Web.Modules.EntriMapping
{
    public class EntriMappingController : AuthorizedBaseController
    {
        public async Task<IActionResult> Index()
        {
           
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            var databaseName = Request.Cookies["DatabaseValue"]; 
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetMapping([DataSourceRequest] DataSourceRequest request, string searchKeyword)
        {
            // Hapus parameter flagClosing di sini agar mengambil SEMUA data
            var data = await Mediator.Send(new GetAllMappingQuery 
            { 
                SearchKeyword = searchKeyword 
            });
            
            return Json(await data.ToDataSourceResultAsync(request));
        }

        public async Task<IActionResult> Add()
        {
            var databaseName = Request.Cookies["DatabaseValue"];
            var model = new EntriMappingViewModel();

            var kodeCabangCookie = Request.Cookies["UserCabang"]?.Trim();
            

            string glDept = null;

            if (!string.IsNullOrEmpty(kodeCabangCookie) && kodeCabangCookie.Length >= 2)
            {
                glDept = kodeCabangCookie.Substring(kodeCabangCookie.Length - 2);
            }

            ViewBag.DebugUserCabang = glDept;
            ViewBag.DebugUserCabang = $"'{glDept}'";

            var akunlist = await Mediator.Send(new GetAllCoaQuery
            {
                KodeCabang = glDept
            });

            
            ViewBag.KodeAkunOptions = akunlist.Select(x => new SelectListItem
            {
                Value = x.Kode,
                Text = $"{x.Kode} - {x.Nama}" 
            }).ToList();

            ////////////////////////
            
            var akunlist117 = await Mediator.Send(new GetAllCoa117Query
            {
                KodeCabang = glDept
            });

            
            ViewBag.KodeAkunOptions117 = akunlist117.Select(x => new SelectListItem
            {
                Value = x.Kode,
                Text = $"{x.Kode} - {x.Nama}" 
            }).ToList();
    



            return PartialView(model);
        }

         [HttpPost]
        public async Task<IActionResult> Save([FromBody] EntriMappingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Jika validasi gagal, kembalikan error 400 dengan detail
                return BadRequest(ModelState);
            }

            var existingData = await Mediator.Send(new GetEntriMappingByIdQuery { gl_akun104 = model.gl_akun104 });
            if (existingData != null)
            {
                await Mediator.Send(Mapper.Map<UpdateEntriMappingCommand>(model));
            }
            else
            {
                await Mediator.Send(Mapper.Map<CreateEntriMappingCommand>(model));
            }
            return Json(new { success = true });
        }

         public async Task<IActionResult> Edit(string gl_akun104)
        {

                var databaseName = Request.Cookies["DatabaseValue"];
                var dto = await Mediator.Send(new GetEntriMappingByIdQuery { gl_akun104 = gl_akun104 });
                if (dto == null)
                {
                    return NotFound();
                }
                var model = Mapper.Map<EntriMappingViewModel>(dto);
                return PartialView(model);
        }

         // Kita ganti nama fungsinya jadi DeleteMapping dan hilangkan [FromBody]
[HttpPost]
public async Task<IActionResult> DeleteMapping(string gl_akun104)
{
    try
    {
        // Pastikan gl_akun104 tidak kosong
        if (string.IsNullOrEmpty(gl_akun104))
        {
            return Json(new { success = false, message = "Kode akun tidak ditemukan!" });
        }

        // Jalankan perintah hapus
        var command = new DeleteEntriMappingCommand { gl_akun104 = gl_akun104 };
        await Mediator.Send(command);
        
        return Json(new { success = true });
    }
    catch (Exception ex)
    {
        // Ambil pesan error paling detail
        string errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        
        // Kembalikan ke layar agar kita bisa baca errornya apa
        return Json(new { success = false, message = errorMsg });
    }
}

    }
}