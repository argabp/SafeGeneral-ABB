using ABB.Application.Common.Interfaces;
using ABB.Application.TarifKebakaranOJKs.Commands;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Web.Modules.TarifKebakaranOJK.Models
{
    public class DetailTarifKebakaranOJKViewModel : IMapFrom<SaveDetailTarifKebakaranOJKCommand>
    {
        public string kd_okup { get; set; }

        public string kd_kls_konstr { get; set; }

        public byte stn_rate_prm { get; set; }

        public decimal pst_rate_prm_min { get; set; }
        
        public decimal pst_rate_prm_max { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DetailTarifKebakaranOJKViewModel, SaveDetailTarifKebakaranOJKCommand>();
            profile.CreateMap<DetailTarifKebakaranOJK, DetailTarifKebakaranOJKViewModel>();
        }
    }
}