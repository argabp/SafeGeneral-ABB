using ABB.Application.CetakNotaPremiTreatyXOLKeluars.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.CetakNotaPremiTreatyXOLKeluar.Models
{
    public class CetakNotaPremiTreatyXOLKeluarViewModel : IMapFrom<CetakNotaPremiTreatyXOLKeluarCommand>
    {
        public string jns_sb_nt { get; set; }
        public string kd_cb { get; set; }
        public string jns_tr { get; set; }
        public string jns_nt_msk { get; set; }
        public string kd_thn { get; set; }
        public string kd_bln { get; set; }
        public string no_nt_msk { get; set; }
        public string jns_nt_kel { get; set; }
        public string no_nt_kel { get; set; }
        public string flag_posting { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CetakNotaPremiTreatyXOLKeluarViewModel, CetakNotaPremiTreatyXOLKeluarCommand>();
        }
    }
}