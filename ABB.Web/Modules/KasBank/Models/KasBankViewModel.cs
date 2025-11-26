using System.ComponentModel.DataAnnotations;
using ABB.Application.Common.Interfaces;
using ABB.Application.KasBanks.Commands;
using ABB.Application.KasBanks.Queries; // <-- Pastikan using ini ada
using AutoMapper;

namespace ABB.Web.Modules.KasBank.Models
{
    public class KasBankViewModel : IMapFrom<CreateKasBankCommand>
    {
        [Required(ErrorMessage = "Kode tidak boleh kosong.")]
        [StringLength(3, ErrorMessage = "Kode maksimal 3 karakter.")]
        [Display(Name = "Kode")]
        public string Kode { get; set; }

        [Required(ErrorMessage = "Keterangan tidak boleh kosong.")]
        [StringLength(75)]
        [Display(Name = "Keterangan")]
        public string Keterangan { get; set; }

        [StringLength(50)]
        [Display(Name = "No. Rekening")]
        public string NoRekening { get; set; }

        [StringLength(50)]
        [Display(Name = "No. Perkiraan")]
        public string NoPerkiraan { get; set; }
        
        [StringLength(4)]
        [Display(Name = "Tipe")]
        public string TipeKasBank { get; set; }

        [Display(Name = "Saldo")]
        public decimal? Saldo { get; set; }

        public void Mapping(Profile profile)
        {
            // Aturan untuk mengirim data DARI form KE command (Untuk Save)
            profile.CreateMap<KasBankViewModel, CreateKasBankCommand>()
                .ForMember(dest => dest.Kasbank, opt => opt.MapFrom(src => src.TipeKasBank));
            profile.CreateMap<KasBankViewModel, UpdateKasBankCommand>();

            // ---> INI BAGIAN PENTING UNTUK EDIT <---
            // Aturan untuk menerima data DARI DTO KE form (Untuk Edit)
            profile.CreateMap<KasBankDto, KasBankViewModel>();
        }
    }
}