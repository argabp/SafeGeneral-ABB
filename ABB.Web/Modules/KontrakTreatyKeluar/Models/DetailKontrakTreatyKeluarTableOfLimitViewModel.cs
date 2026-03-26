using ABB.Application.Common.Interfaces;
using ABB.Application.KontrakTreatyKeluars.Commands;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Web.Modules.KontrakTreatyKeluar.Models
{
    public class DetailKontrakTreatyKeluarTableOfLimitViewModel : IMapFrom<SaveDetailKontrakTreatyKeluarTableOfLimitCommand>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public string kd_okup { get; set; }

        public string category_rsk { get; set; }

        public string kd_kls_konstr { get; set; }
        
        public decimal pst_bts_tty { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DetailKontrakTreatyKeluarTableOfLimit, DetailKontrakTreatyKeluarTableOfLimitViewModel>();
            profile.CreateMap<DetailKontrakTreatyKeluarTableOfLimitViewModel, SaveDetailKontrakTreatyKeluarTableOfLimitCommand>();
        }
    }
}