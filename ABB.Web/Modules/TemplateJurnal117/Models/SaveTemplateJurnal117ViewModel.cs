using ABB.Application.Common.Interfaces;
using ABB.Application.TemplateJurnals117.Commands;
using AutoMapper;

namespace ABB.Web.Modules.TemplateJurnal117.Models
{
    public class SaveTemplateJurnal117ViewModel : IMapFrom<AddTemplateJurnal117Command>
    {
        public string type_tr { get; set; }
        public string type_jr { get; set; }
        public string metode { get; set; }
        public string Event { get; set; }
        public string jn_ass { get; set; }
        public string nm_jr { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveTemplateJurnal117ViewModel, AddTemplateJurnal117Command>();
            profile.CreateMap<SaveTemplateJurnal117ViewModel, EditTemplateJurnal117Command>();
        }
    }
}