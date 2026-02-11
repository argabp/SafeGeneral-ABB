using System.ComponentModel.DataAnnotations;
using ABB.Application.Common.Interfaces;
using ABB.Application.VoucherKass.Commands;
using ABB.Application.VoucherKass.Queries; // <-- Pastikan using ini ada
using AutoMapper;
using System;
namespace ABB.Web.Modules.VoucherKas.Models
{
    public class VoucherKasViewModel : IMapFrom<CreateVoucherKasCommand>
    {
        public long Id { get; set; }
        
        [Display(Name = "KodeCabang")]
        public string KodeCabang { get; set; }

        // --- TAMBAHKAN INI ---
        [Display(Name = "Kode Kas")]
        public string KodeKas { get; set; } 
        // ---------------------

        
        [Display(Name = "JenisVoucher")]
        public string JenisVoucher { get; set; }

        [StringLength(10)]
        [Display(Name = "Debet/Kredit")]
        public string DebetKredit { get; set; }

        [StringLength(50)]
        [Display(Name = "NoVoucher")]
        public string NoVoucher { get; set; }

         [Display(Name = "KodeAkun")]
        public string KodeAkun { get; set; }

        [Display(Name = "DibayarKepada")]
        public string DibayarKepada { get; set; }

        [Required(ErrorMessage = "Tanggal tidak boleh kosong.")]
        [Display(Name = "TanggalVoucher")]
        public DateTime? TanggalVoucher { get; set; }

        [Required(ErrorMessage = "Total tidak boleh kosong.")]
        [Display(Name = "TotalVoucher")]
        public decimal? TotalVoucher { get; set; }

        [Required(ErrorMessage = "Kode Mata Uang tidak boleh kosong.")]
        [Display(Name = "KodeMataUang")]
        public string KodeMataUang { get; set; }

     
        
        public string KeteranganVoucher { get; set; }

        [Required(ErrorMessage = "Total tidak boleh kosong.")]
        [Display(Name = "TotalDalamRupiah")]
        public decimal? TotalDalamRupiah { get; set; }

        [Display(Name = "Flag Posting")]
        public bool FlagPosting { get; set; }

      
        [Display(Name = "TanggalInput")]
        public DateTime? TanggalInput { get; set; }

      
        [Display(Name = "TanggalUpdate")]
        public DateTime? TanggalUpdate { get; set; }

         [Display(Name = "KodeUserInput")]
        public string KodeUserInput { get; set; }

        [Display(Name = "KodeUserUpdate")]
        public string KodeUserUpdate { get; set; }

        [Display(Name = "Jenis Pembayaran")]
        public string JenisPembayaran { get; set; }

        [Display(Name = "Flag Sementara")]
        public bool FlagSementara { get; set; }

        [Display(Name = "No Voucher Sementara")]
        public string NoVoucherSementara { get; set; }

        // --- [TAMBAHKAN INI] ---
        [Display(Name = "Flag Final")]
        public bool FlagFinal { get; set; } 
        // -----------------------

        public void Mapping(Profile profile)
        {
            // Aturan untuk mengirim data DARI form KE command (Untuk Save)
            profile.CreateMap<VoucherKasDto, VoucherKasViewModel>();
            profile.CreateMap<VoucherKasViewModel, CreateVoucherKasCommand>();
            profile.CreateMap<VoucherKasViewModel, UpdateVoucherKasCommand>();

            // ---> INI BAGIAN PENTING UNTUK EDIT <---
            // Aturan untuk menerima data DARI DTO KE form (Untuk Edit)
            
        } 
    }
}