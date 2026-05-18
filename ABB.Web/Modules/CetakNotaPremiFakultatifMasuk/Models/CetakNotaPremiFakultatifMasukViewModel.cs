using ABB.Application.CetakNotaPremiFakultatifMasuks.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.CetakNotaPremiFakultatifMasuk.Models
{
    public class CetakNotaPremiFakultatifMasukViewModel : IMapFrom<CetakNotaPremiFakultatifMasukCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public short no_updt { get; set; }

        public string jns_lap { get; set; }

        public string kd_mtu { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CetakNotaPremiFakultatifMasukViewModel, CetakNotaPremiFakultatifMasukCommand>();
        }
    }
}