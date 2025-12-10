using ABB.Application.Common.Interfaces;
using ABB.Application.TemplateJurnals117.Commands;
using AutoMapper;

namespace ABB.Web.Modules.TemplateJurnal117.Models
{
    public class SaveTemplateJurnal117ViewModel : IMapFrom<AddTemplateJurnal117Command>
    {
        public string Type { get; set; }

        public string JenisAss { get; set; }

        public string NamaJurnal { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveTemplateJurnal117ViewModel, AddTemplateJurnal117Command>();
            profile.CreateMap<SaveTemplateJurnal117ViewModel, EditTemplateJurnal117Command>();
        }
    }
}