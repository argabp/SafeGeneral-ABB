using System.ComponentModel.DataAnnotations;
using ABB.Application.Common.Interfaces;
using ABB.Application.VoucherKass.Commands;
using ABB.Application.EntriPembayaranKass.Commands;
using ABB.Application.VoucherKass.Queries; // <-- Pastikan using ini ada
using ABB.Application.EntriPembayaranKass.Queries;
using AutoMapper;
using System;
using System.Collections.Generic;

namespace ABB.Web.Modules.EntriPembayaranKas.Models
{
    public class EntriPembayaranKasViewModel : IMapFrom<CreatePembayaranKasCommand>
    {
      
        
        public VoucherKasDto VoucherKasHeader { get; set; }
        public List<EntriPembayaranKasDto> Details { get; set; } = new List<EntriPembayaranKasDto>();

        public string NoVoucher { get; set; }
        public int No { get; set; }


        // Field tambahan
        [Display(Name = "Flag Pembayaran")]
        public string FlagPembayaran { get; set; }
        
        [Display(Name = "DebetKredit")]
        public string DebetKredit { get; set; }
        
        [Display(Name = "Nomor Nota")]
        public string NoNota4 { get; set; }

        [Display(Name = "Kode Akun")]
        public string KodeAkun { get; set; }

        [Display(Name = "Total Bayar")]
        public decimal? TotalBayar { get; set; }

        [Display(Name = "Kode Mata Uang")]
        public string KodeMataUang { get; set; }

        [Display(Name = "Total Dalam Rupiah")]
        public decimal? TotalDlmRupiah { get; set; }

         [Display(Name = "Kurs")]
        public int? Kurs { get; set; }
        // Konfigurasi AutoMapper
        public void Mapping(Profile profile)
        {
            profile.CreateMap<EntriPembayaranKasViewModel, CreatePembayaranKasCommand>();
            profile.CreateMap<EntriPembayaranKasViewModel, UpdatePembayaranKasCommand>();
             profile.CreateMap<EntriPembayaranKasViewModel, UpdateFinalPembayaranKasCommand>();
        }
    }
}