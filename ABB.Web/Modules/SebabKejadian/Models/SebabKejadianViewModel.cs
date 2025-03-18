using ABB.Application.Common.Interfaces;
using ABB.Application.SebabKejadians.Commands;
using ABB.Application.SebabKejadians.Queries;
using AutoMapper;

namespace ABB.Web.Modules.SebabKejadian.Models
{
    public class SebabKejadianViewModel : IMapFrom<SaveSebabKejadianCommand>
    {
        public string kd_cob { get; set; }

        public string kd_sebab { get; set; }

        public string nm_sebab { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SebabKejadianViewModel, SaveSebabKejadianCommand>();
            profile.CreateMap<SebabKejadianDto, SebabKejadianViewModel>();
        }
    }
}