using ABB.Application.CetakLampiranEndorsments.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.CetakLampiranEndorsment.Models
{
    public class CetakLampiranEndorsmentViewModel : IMapFrom<GetCetakLampiranEndorsmentQuery>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string no_pol { get; set; }
        public string kd_thn { get; set; }
        public int no_updt { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CetakLampiranEndorsmentViewModel, GetCetakLampiranEndorsmentQuery>();
        }
    }
}