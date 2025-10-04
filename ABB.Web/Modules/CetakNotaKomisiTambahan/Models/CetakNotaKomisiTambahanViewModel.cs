using ABB.Application.CetakNotaKomisiTambahans.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.CetakNotaKomisiTambahan.Models
{
    public class CetakNotaKomisiTambahanViewModel : IMapFrom<GetCetakNotaKomisiTambahanQuery>
    {
        public string kd_cb { get; set; }
        public string jns_tr { get; set; }
        public string jns_nt_msk { get; set; }
        public string kd_thn { get; set; }
        public string kd_bln { get; set; }
        public string no_nt_msk { get; set; }
        public string jns_nt_kel { get; set; }
        public string no_nt_kel { get; set; }
        public string flag_posting { get; set; }
        public string jns_sb_nt { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CetakNotaKomisiTambahanViewModel, GetCetakNotaKomisiTambahanQuery>();
        }
    }
}