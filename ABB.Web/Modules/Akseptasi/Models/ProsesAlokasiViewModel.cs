using System;
using ABB.Application.Alokasis.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Akseptasi.Models
{
    public class ProsesAlokasiViewModel : IMapFrom<ProsesAlokasiCommand>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_pol { get; set; }
        public string no_updt { get; set; }
        public DateTime tgl_closing { get; set; }
        public string st_tty { get; set; }
        public string flag_survey { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProsesAlokasiViewModel, ProsesAlokasiCommand>();
        }
    }
}