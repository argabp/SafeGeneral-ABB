using System.ComponentModel.DataAnnotations;
using ABB.Application.Common.Interfaces;
using ABB.Application.Coas.Commands;
using ABB.Application.Coas.Queries; // <-- Pastikan using ini ada
using AutoMapper;
using System;
namespace ABB.Web.Modules.COA.Models
{
    public class CoaViewModel : IMapFrom<CreateCoaCommand>
    {
      
        [Display(Name = "Kode")]
        public string Kode { get; set; }

        
        [Display(Name = "Nama")]
        public string Nama { get; set; }

        [Display(Name = "Lokasi")]
        public string Dept { get; set; }

        [Display(Name = "Type")]
        public string Type { get; set; }

        public void Mapping(Profile profile)
        {
            // Aturan untuk mengirim data DARI form KE command (Untuk Save)
            profile.CreateMap<CoaDto, CoaViewModel>();
            profile.CreateMap<CoaViewModel, CreateCoaCommand>();
            profile.CreateMap<CoaViewModel, UpdateCoaCommand>();
            
        } 
    }
}