using System.ComponentModel.DataAnnotations;
using ABB.Application.Common.Interfaces;
using ABB.Application.TipeAkuns104.Commands;
using ABB.Application.TipeAkuns104.Queries; 
using AutoMapper;
using System;
namespace ABB.Web.Modules.TipeAkun104.Models
{
    public class TipeAkun104ViewModel : IMapFrom<CreateTipeAkun104Command>
    {
      
        [Display(Name = "Kode")]
        public string Kode { get; set; }

        
        [Display(Name = "Nama Tipe")]
        public string NamaTipe { get; set; }

        [Display(Name = "POS")]
        public string Pos { get; set; }

        [Display(Name = "Kasbank")]
        public bool? Kasbank { get; set; }

        public void Mapping(Profile profile)
        {
            // Aturan untuk mengirim data DARI form KE command (Untuk Save)
            profile.CreateMap<TipeAkun104Dto, TipeAkun104ViewModel>();
            profile.CreateMap<TipeAkun104ViewModel, CreateTipeAkun104Command>();
            // profile.CreateMap<TipeAkun104ViewModel, UpdateTipeAkun104Command>();
            
        } 
    }
}