using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.ProsesPremiXOLKeluars.Commands;
using AutoMapper;

namespace ABB.Web.Modules.ProsesPremiXOLKeluar.Models
{
    public class ProsesPremiXOLKeluarModel : IMapFrom<ProsesPremiXOLKeluarCommand>
    {
        public string kd_cb { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_npps { get; set; }

        public string kd_mtu { get; set; }

        public string no_tr { get; set; }

        public DateTime tgl_closing { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProsesPremiXOLKeluarModel, ProsesPremiXOLKeluarCommand>();
            profile.CreateMap<ProsesPremiXOLKeluarModel, CancelProsesPremiXOLKeluarCommand>();
        }
    }
}