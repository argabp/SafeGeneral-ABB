using System.ComponentModel.DataAnnotations;
using ABB.Application.Common.Interfaces;
using ABB.Application.TipeAkuns117.Commands; // Pastikan using ini ada
using ABB.Application.TipeAkuns117.Queries;
using AutoMapper;

namespace ABB.Web.Modules.TipeAkun117.Models
{
    public class TipeAkun117ViewModel : IMapFrom<TipeAkun117Dto>, IMapFrom<CreateTipeAkun117Command>, IMapFrom<UpdateTipeAkun117Command>
    {
        [Required(ErrorMessage = "Kode harus diisi")]
        [Display(Name = "Kode Tipe")]
        public string Kode { get; set; }

        [Required(ErrorMessage = "Nama Tipe harus diisi")]
        [Display(Name = "Nama Tipe")]
        public string NamaTipe { get; set; }

        [Display(Name = "Posisi")]
        public string Pos { get; set; } // Contoh: D (Debet) / K (Kredit) atau Neraca/LabaRugi

        [Display(Name = "Debet/Kredit")]
        public string DebetKredit { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TipeAkun117Dto, TipeAkun117ViewModel>();
            profile.CreateMap<TipeAkun117ViewModel, CreateTipeAkun117Command>();
            profile.CreateMap<TipeAkun117ViewModel, UpdateTipeAkun117Command>();
        }
    }
}