using System.ComponentModel.DataAnnotations;
using ABB.Application.Common.Interfaces;
using ABB.Application.Coas117.Commands;
using ABB.Application.Coas117.Queries; // <-- Pastikan using ini ada
using AutoMapper;
using System;
namespace ABB.Web.Modules.COA117.Models
{
    public class Coa117ViewModel : IMapFrom<CreateCoa117Command>
    {
      
        [Display(Name = "Kode")]
        public string Kode { get; set; }

        
        [Display(Name = "Nama")]
        public string Nama { get; set; }

        [Display(Name = "Dept")]
        public string Dept { get; set; }

        [Display(Name = "Type")]
        public string Type { get; set; }

        public void Mapping(Profile profile)
        {
            // Aturan untuk mengirim data DARI form KE command (Untuk Save)
            profile.CreateMap<Coa117Dto, Coa117ViewModel>();
            profile.CreateMap<Coa117ViewModel, CreateCoa117Command>();
            profile.CreateMap<Coa117ViewModel, UpdateCoa117Command>();
            
        } 
    }
}