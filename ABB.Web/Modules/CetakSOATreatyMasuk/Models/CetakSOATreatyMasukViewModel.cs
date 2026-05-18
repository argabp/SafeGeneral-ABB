using ABB.Application.CetakSOATreatyMasuks.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.CetakSOATreatyMasuk.Models
{
    public class CetakSOATreatyMasukViewModel : IMapFrom<CetakSOATreatyMasukCommand>
    {
        public string kd_cb { get; set; }
        
        public string kuartal_tr { get; set; }

        public string thn_tr { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CetakSOATreatyMasukViewModel, CetakSOATreatyMasukCommand>();
        }
    }
}