using System;
using ABB.Application.Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using ABB.Application.EntriPembayaranBanks.Commands;
using ABB.Application.VoucherBanks.Queries;
using ABB.Application.VoucherBanks.Commands;
using AutoMapper;
using System.Collections.Generic;

namespace ABB.Web.Modules.EntriPembayaranBank.Models
{
    public class PembayaranBankItem
    {
        public int No { get; set; }
        public DateTime? TglBayar { get; set; }
        public string KodeAkun { get; set; }
        public int? TotalBayar { get; set; }
        public string DebetKredit { get; set; }
        public string KeteranganVoucher { get; set; } // Asumsi keterangan per baris
    }

    public class EntriPembayaranBankViewModel : IMapFrom<CreatePembayaranBankCommand>
    {
        // ======================================================
        // Bagian 1: Data Header Voucher (Hanya untuk Tampilan)
        // ======================================================
        public VoucherBankDto VoucherHeader { get; set; }

        // ======================================================
        // Bagian 2: Properti untuk Form Input Pembayaran Baru
        // ======================================================
        public List<PembayaranBankItem> PembayaranItems { get; set; } = new List<PembayaranBankItem>();
        
        // NoVoucher akan diisi otomatis dan disembunyikan
        public string NoVoucher { get; set; }
         public int No { get; set; }

        [Display(Name = "Flag Pembayaran")]
        public string FlagPembayaran { get; set; }
        
        
         
        [Display(Name = "Nomor Nota")]
        public string NoNota4 { get; set; }
        
        // Field input lainnya
       
        
        [Display(Name = "Kode Akun")]
        public string KodeAkun { get; set; }

        [Display(Name = "Debit Kredit")]
        public string DebetKredit { get; set; }
        
        [Display(Name = "Total Bayar")]
        public int? TotalBayar { get; set; }

        [Display(Name = "Kode Mata Uang")]
        public string KodeMataUang { get; set; }

        [Display(Name = "Total Dalam Rupiah")]
        public decimal? TotalDlmRupiah { get; set; }
       
        [Display(Name = "Nilai Kurs")]
        public decimal? NilaiKurs { get; set; }

        [Display(Name = "Kurs")]
        public int? Kurs { get; set; }

         // --- [TAMBAHKAN INI] ---
        [Display(Name = "Flag Final")]
        public bool FlagFinal { get; set; } 
        // -----------------------

        // Anda bisa tambahkan properti lain dari abb_pembayaran_bank di sini jika perlu diinput
        // Contoh: public string FlagPembayaran { get; set; }

        // Konfigurasi AutoMapper
        public void Mapping(Profile profile)
        {
            // Aturan untuk mengubah ViewModel (input) menjadi Command (perintah simpan)
            profile.CreateMap<EntriPembayaranBankViewModel, CreatePembayaranBankCommand>();
            profile.CreateMap<EntriPembayaranBankViewModel, UpdatePembayaranBankCommand>();
            profile.CreateMap<EntriPembayaranBankViewModel, UpdatePembayaranBankLihatCommand>();
        }
    }
}