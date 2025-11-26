using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.VoucherKass.Queries;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;
using ABB.Web.Modules.ListVoucherKas.Models;

namespace ABB.Web.Modules.ListVoucherKas
{
    public class ListVoucherKasController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            return View();
        }

        // [HttpGet]
        // public async Task<IActionResult> CetakVoucherKas(DateTime tanggalAwal, DateTime tanggalAkhir)
        // {
        //     if (tanggalAwal > tanggalAkhir)
        //     {
        //         var tmp = tanggalAwal;
        //         tanggalAwal = tanggalAkhir;
        //         tanggalAkhir = tmp;
        //     }

        //     var vouchers = await Mediator.Send(new GetVoucherKasByTanggalRangeQuery
        //     {
        //         TanggalAwal = tanggalAwal,
        //         TanggalAkhir = tanggalAkhir
        //     });

        //     var viewModel = new ListVoucherKasViewModel
        //     {
        //         TanggalAwal = tanggalAwal,
        //         TanggalAkhir = tanggalAkhir,
        //         VoucherList = vouchers
        //     };

        //     // Langsung kirim ke view cetak (paper CSS)
        //     return PartialView("PrintVoucherKas", viewModel);
        // }
    }
}
