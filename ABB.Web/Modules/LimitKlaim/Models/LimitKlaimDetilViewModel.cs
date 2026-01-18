using ABB.Application.Common.Interfaces;
using ABB.Application.LimitKlaims.Commands;
using ABB.Application.LimitKlaims.Queries;
using AutoMapper;

namespace ABB.Web.Modules.LimitKlaim.Models
{
    public class LimitKlaimDetilViewModel : IMapFrom<AddLimitKlaimDetilCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }

        public string kd_user { get; set; }

        public decimal nilai_limit_awal { get; set; }
        
        public decimal nilai_limit_akhir { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LimitKlaimDetilViewModel, AddLimitKlaimDetilCommand>();
            profile.CreateMap<LimitKlaimDetilViewModel, EditLimitKlaimDetilCommand>();
            profile.CreateMap<LimitKlaimDetilDto, LimitKlaimDetilViewModel>();
        }
    }
}