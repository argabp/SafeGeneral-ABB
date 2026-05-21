using ABB.Application.Common.Interfaces;
using ABB.Application.TemplateJurnals117.Commands;
using AutoMapper;

namespace ABB.Web.Modules.TemplateJurnal117.Models
{
    public class DeleteTemplateJurnal117ViewModel : IMapFrom<DeleteTemplateJurnal117Command>
    {
        public string type_tr { get; set; }
        public string type_jr { get; set; }
        public string metode { get; set; }
        public string Event { get; set; }
        public string jn_ass { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteTemplateJurnal117ViewModel, DeleteTemplateJurnal117Command>();
        }
    }
}