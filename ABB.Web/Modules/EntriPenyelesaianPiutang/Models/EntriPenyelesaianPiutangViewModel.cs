using System;
using ABB.Application.Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using ABB.Application.EntriPenyelesaianPiutangs.Commands;
using ABB.Application.EntriPenyelesaianPiutangs.Queries;
using AutoMapper;
using System.Collections.Generic;

namespace ABB.Web.Modules.EntriPenyelesaianPiutang.Models
{
     public class PenyelesaianPiutangItem
    {
        public int No { get; set; }
        public string FlagPembayaran { get; set; }
        public string KodeAkun { get; set; }
        
        public string NoNota { get; set; }
        public string NoBukti { get; set; }
        public string KodeMataUang { get; set; }
        public decimal? TotalBayarOrg { get; set; }
        public decimal? TotalBayarRp { get; set; }
        public string DebetKredit { get; set; }
        public string UserBayar { get; set; }
        
    }

    public class EntriPenyelesaianPiutangViewModel : IMapFrom<CreatePenyelesaianPiutangCommand>
    {
         // Properti untuk data header
        public HeaderPenyelesaianUtangDto PenyelesaianHeader { get; set; }

        // Properti untuk list data detail (grid)
        public List<PenyelesaianPiutangItem> PembayaranItems { get; set; }

        public EntriPenyelesaianPiutangViewModel()
        {
            // Ini adalah baris kunci yang memperbaiki error.
            // Saat ViewModel dibuat, Header-nya juga langsung dibuat (tidak null lagi).
            PenyelesaianHeader = new HeaderPenyelesaianUtangDto(); 

            // Sebaiknya inisialisasi list juga di sini agar konsisten.
            PembayaranItems = new List<PenyelesaianPiutangItem>();
        }

        public string NoBukti { get; set; }
         public int No { get; set; }

        [Display(Name = "Flag Pembayaran")]
        public string FlagPembayaran { get; set; }
        
         [Display(Name = "Kode Cabang")]
        public string KodeCabang { get; set; }
        
         
        [Display(Name = "Nomor Nota")]
        public string NoNota { get; set; }
        
        // Field input lainnya
       
        
        [Display(Name = "Kode Akun")]
        public string KodeAkun { get; set; }

        [Display(Name = "Debit Kredit")]
        public string DebetKredit { get; set; }
        
        [Display(Name = "Total Bayar")]
        public decimal? TotalBayarOrg { get; set; }

        [Display(Name = "Total Bayar")]
        public decimal? TotalBayarRp { get; set; }

        [Display(Name = "Kode Mata Uang")]
        public string KodeMataUang { get; set; }
        
        [Display(Name = "User Bayar")]
        public string UserBayar { get; set; }

         [Display(Name = "Flag Final")]
        public bool? FlagFinal { get; set; }

        // Anda bisa tambahkan properti lain dari abb_pembayaran_bank di sini jika perlu diinput
        // Contoh: public string FlagPembayaran { get; set; }

        // Konfigurasi AutoMapper
        public void Mapping(Profile profile)
        {
            // Aturan untuk mengubah ViewModel (input) menjadi Command (perintah simpan)
            profile.CreateMap<EntriPenyelesaianPiutangViewModel, CreatePenyelesaianPiutangCommand>();
            profile.CreateMap<EntriPenyelesaianPiutangViewModel, UpdatePenyelesaianPiutangCommand>();
            profile.CreateMap<HeaderPenyelesaianUtangDto, CreateHeaderPenyelesaianUtangCommand>();
            profile.CreateMap<EntriPenyelesaianPiutangViewModel, UpdateFinalPenyelesaianPiutangCommand>();
            profile.CreateMap<EntriPenyelesaianPiutangDto, PenyelesaianPiutangItem>();
        }
    }
}