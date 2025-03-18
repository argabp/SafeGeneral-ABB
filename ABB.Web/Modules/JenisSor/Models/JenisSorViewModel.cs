using ABB.Application.Common.Interfaces;
using ABB.Application.JenisSors.Commands;
using ABB.Application.JenisSors.Queries;
using AutoMapper;

namespace ABB.Web.Modules.JenisSor.Models
{
    public class JenisSorViewModel : IMapFrom<SaveJenisSorCommand>
    {
        public string kd_jns_sor { get; set; }

        public string nm_jns_sor { get; set; }

        public string grp_jns_sor { get; set; }

        public string? no_urut { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<JenisSorViewModel, SaveJenisSorCommand>();
            profile.CreateMap<JenisSorDto, JenisSorViewModel>();
        }
    }
}