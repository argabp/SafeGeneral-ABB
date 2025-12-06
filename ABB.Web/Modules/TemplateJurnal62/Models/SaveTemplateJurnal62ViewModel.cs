using ABB.Application.Common.Interfaces;
using ABB.Application.TemplateJurnals62.Commands;
using AutoMapper;

namespace ABB.Web.Modules.TemplateJurnal62.Models
{
    public class SaveTemplateJurnal62ViewModel : IMapFrom<AddTemplateJurnal62Command>
    {
        public string Type { get; set; }

        public string JenisAss { get; set; }

        public string NamaJurnal { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveTemplateJurnal62ViewModel, AddTemplateJurnal62Command>();
            profile.CreateMap<SaveTemplateJurnal62ViewModel, EditTemplateJurnal62Command>();
        }
    }
}