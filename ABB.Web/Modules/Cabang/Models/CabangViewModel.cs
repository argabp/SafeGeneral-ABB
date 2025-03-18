using ABB.Application.Cabangs.Commands;
using ABB.Application.Cabangs.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Cabang.Models
{
    public class CabangViewModel : IMapFrom<CabangDto>
    {
        public string kd_cb { get; set; }

        public string nm_cb { get; set; }

        public string almt { get; set; }

        public string kt { get; set; }

        public string kd_pos { get; set; }

        public string no_tlp { get; set; }

        public string npwp { get; set; }

        public string no_fax { get; set; }

        public string email { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CabangDto, CabangViewModel>();
            profile.CreateMap<CabangViewModel, AddCabangCommand>();
            profile.CreateMap<CabangViewModel, EditCabangCommand>();
        }
    }
}