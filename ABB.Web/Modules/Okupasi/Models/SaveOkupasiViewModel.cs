using ABB.Application.Common.Interfaces;
using ABB.Application.Okupasis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Okupasi.Models
{
    public class SaveOkupasiViewModel : IMapFrom<AddOkupasiCommand>
    {
        public string kd_okup { get; set; }

        public string nm_okup { get; set; }

        public string nm_okup_ing { get; set; }

        public string kd_category { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveOkupasiViewModel, AddOkupasiCommand>();
            profile.CreateMap<SaveOkupasiViewModel, EditOkupasiCommand>();
        }
    }
}