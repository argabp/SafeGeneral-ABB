using ABB.Application.Common.Interfaces;
using ABB.Application.KontrakTreatyKeluars.Commands;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Web.Modules.KontrakTreatyKeluar.Models
{
    public class DetailKontrakTreatyKeluarKoasuransiViewModel : IMapFrom<SaveDetailKontrakTreatyKeluarKoasuransiCommand>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public int no_urut { get; set; }

        public decimal pst_share_mul { get; set; }

        public decimal pst_share_akh { get; set; }
        
        public decimal pst_bts_koas { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DetailKontrakTreatyKeluarKoasuransi, DetailKontrakTreatyKeluarKoasuransiViewModel>();
            profile.CreateMap<DetailKontrakTreatyKeluarKoasuransiViewModel, SaveDetailKontrakTreatyKeluarKoasuransiCommand>();
        }
    }
}