using System.ComponentModel.DataAnnotations;
using ABB.Application.Common.Interfaces;
using ABB.Application.TypeCoas.Commands;
using ABB.Application.TypeCoas.Queries; 
using AutoMapper;
using System;
namespace ABB.Web.Modules.TypeCoa.Models
{
    public class TypeCoaViewModel : IMapFrom<CreateTypeCoaCommand>
    {
      
        [Display(Name = "Kode")]
        public string Type { get; set; }

        
        [Display(Name = "Nama Tipe")]
        public string Nama { get; set; }

        [Display(Name = "POS")]
        public string Pos { get; set; }

        [Display(Name = "Debit/Kredit")]
        public string Dk { get; set; }

        public void Mapping(Profile profile)
        {
            // Aturan untuk mengirim data DARI form KE command (Untuk Save)
            profile.CreateMap<TypeCoaDto, TypeCoaViewModel>();
            profile.CreateMap<TypeCoaViewModel, CreateTypeCoaCommand>();
            profile.CreateMap<TypeCoaViewModel, UpdateTypeCoaCommand>();
            
        } 
    }
}