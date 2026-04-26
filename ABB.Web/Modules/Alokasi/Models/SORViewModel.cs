using ABB.Application.Alokasis.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Alokasi.Models
{
    public class SORViewModel : IMapFrom<GetAlokasiQuery>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public short no_updt { get; set; }

        public bool? IsViewOnly { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SORViewModel, GetAlokasiQuery>();
            profile.CreateMap<SORViewModel, GetDetailAlokasiQuery>();
        }
    }
}