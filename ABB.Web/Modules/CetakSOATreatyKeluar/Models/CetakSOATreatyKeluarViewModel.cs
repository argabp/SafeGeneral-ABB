using ABB.Application.CetakSOATreatyKeluars.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.CetakSOATreatyKeluar.Models
{
    public class CetakSOATreatyKeluarViewModel : IMapFrom<CetakSOATreatyKeluarCommand>
    {
        public string kd_cb { get; set; }
        
        public string kuartal_tr { get; set; }

        public string thn_tr { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }

        public string jns_laporan { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CetakSOATreatyKeluarViewModel, CetakSOATreatyKeluarCommand>();
        }
    }
}