using ABB.Application.Akuisisis.Commands;
using ABB.Application.Akuisisis.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Akuisisi.Models
{
    public class AkuisisiViewModel : IMapFrom<SaveAkuisisiCommand>
    {
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int kd_thn { get; set; }

        public string kd_mtu { get; set; }

        public decimal nilai_min_acq { get; set; }

        public decimal nilai_maks_acq { get; set; }

        public decimal nilai_acq { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AkuisisiViewModel, SaveAkuisisiCommand>();
            profile.CreateMap<AkuisisiDto, AkuisisiViewModel>();
        }
    }
}