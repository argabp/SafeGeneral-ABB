using ABB.Application.Common.Interfaces;
using ABB.Application.TemplateJurnals117.Commands;
using AutoMapper;

namespace ABB.Web.Modules.TemplateJurnal117.Models
{
    public class DeleteTemplateJurnal117ViewModel : IMapFrom<DeleteTemplateJurnal117Command>
    {
        public string Type { get; set; }
        public string JenisAss { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteTemplateJurnal117ViewModel, DeleteTemplateJurnal117Command>();
        }
    }
}