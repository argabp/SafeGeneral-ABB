using System.ComponentModel.DataAnnotations;
using ABB.Application.Common.Interfaces;
using ABB.Application.EntriMappings.Commands;
using ABB.Application.EntriMappings.Queries; 
using AutoMapper;

namespace ABB.Web.Modules.EntriMapping.Models
{
    public class EntriMappingViewModel : IMapFrom<CreateEntriMappingCommand>
    {
        [Display(Name = "gl_akun104")]
        public string gl_akun104 { get; set; }

        [Display(Name = "gl_akun117")]
        public string gl_akun117 { get; set; }

        public void Mapping(Profile profile)
        {
            // 1. Dari DTO ke ViewModel (Untuk memuat data saat Edit)
            profile.CreateMap<EntriMappingDto, EntriMappingViewModel>();
            
            // 2. PERBAIKAN: Dari ViewModel ke Command (Untuk proses Save / Create / Update)
            profile.CreateMap<EntriMappingViewModel, CreateEntriMappingCommand>();
            profile.CreateMap<EntriMappingViewModel, UpdateEntriMappingCommand>();
        } 
    }
}