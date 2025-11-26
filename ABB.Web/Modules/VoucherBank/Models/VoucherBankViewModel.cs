using System;
using System.ComponentModel.DataAnnotations;
using ABB.Application.Common.Interfaces;
using ABB.Application.VoucherBanks.Queries;
using ABB.Application.VoucherBanks.Commands;
using AutoMapper;

namespace ABB.Web.Modules.VoucherBank.Models
{
    public class VoucherBankViewModel : IMapFrom<CreateVoucherBankCommand>
    {
        [Required]
        [StringLength(4)]
        [Display(Name = "Kode Cabang")]
        public string KodeCabang { get; set; }
        
        [Required]
        [StringLength(4)]
        [Display(Name = "Jenis Voucher")]
        public string JenisVoucher { get; set; }

        [Display(Name = "Jenis Pembayaran")]
        public string JenisPembayaran { get; set; }

        [StringLength(6)]
        [Display(Name = "Debet/Kredit")]
        public string DebetKredit { get; set; }
        
        [Required]
        [StringLength(50)]
        [Display(Name = "No. Voucher")]
        public string NoVoucher { get; set; }

        [StringLength(10)]
        [Display(Name = "Kode Akun")]
        public string KodeAkun { get; set; }

        [StringLength(100)]
        [Display(Name = "Diterima Dari")]
        public string DiterimaDari { get; set; }

        [Display(Name = "Tanggal Voucher")]
        public DateTime? TanggalVoucher { get; set; }

        [StringLength(3)]
        [Display(Name = "Mata Uang")]
        public string KodeMataUang { get; set; }

        [Display(Name = "Total Voucher")]
        public decimal? TotalVoucher { get; set; }
        
        [Display(Name = "Total Dalam Rupiah")]
        public decimal? TotalDalamRupiah { get; set; }

        [StringLength(100)]
        [Display(Name = "Keterangan")]
        public string KeteranganVoucher { get; set; }

        
        [Display(Name = "Flag Posting")]
        public bool FlagPosting { get; set; }

        [Display(Name = "Flag Final")]
        public bool FlagFinal { get; set; }
        
        [StringLength(5)]
        [Display(Name = "Kode Bank")]
        public string KodeBank { get; set; }

        [StringLength(255)]
        [Display(Name = "No. Bank")]
        public string NoBank { get; set; }

        [Display(Name = "Tanggal Input")] // Khusus untuk ViewModel
        public DateTime? TanggalInput { get; set; }

        [Display(Name = "User Input")] // Khusus untuk ViewModel
        public string KodeUserInput { get; set; }

        [Display(Name = "Tanggal Update")] // Khusus untuk ViewModel
        public DateTime? TanggalUpdate { get; set; }

        [Display(Name = "User Update")] // Khusus untuk ViewModel
        public string KodeUserUpdate { get; set; }
        
        public string NamaUserInput { get; set; }
        public string NamaUserUpdate { get; set; }

        // Konfigurasi AutoMapper
        public void Mapping(Profile profile)
        {
            // Aturan untuk menampilkan data KE form (Edit)
            profile.CreateMap<VoucherBankDto, VoucherBankViewModel>();

            profile.CreateMap<VoucherBankViewModel, CreateVoucherBankCommand>();
            profile.CreateMap<VoucherBankViewModel, UpdateVoucherBankCommand>();


        }
    }
}