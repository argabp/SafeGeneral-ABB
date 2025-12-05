using ABB.Application.Common.Interfaces;
using ABB.Application.TemplateJurnals62.Commands;
using AutoMapper;

namespace ABB.Web.Modules.TemplateJurnal62.Models
{
    public class DeleteTemplateJurnal62ViewModel : IMapFrom<DeleteTemplateJurnal62Command>
    {
        public string Type { get; set; }
        public string JenisAss { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteTemplateJurnal62ViewModel, DeleteTemplateJurnal62Command>();
        }
    }
}