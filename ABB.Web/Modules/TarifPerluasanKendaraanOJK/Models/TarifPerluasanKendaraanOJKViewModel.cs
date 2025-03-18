using ABB.Application.Common.Interfaces;
using ABB.Application.TarifPerluasanKendaraanOJKs.Commands;
using ABB.Application.TarifPerluasanKendaraanOJKs.Queries;
using AutoMapper;

namespace ABB.Web.Modules.TarifPerluasanKendaraanOJK.Models
{
    public class TarifPerluasanKendaraanOJKViewModel : IMapFrom<SaveTarifPerluasanKendaraanOJKCommand>
    {
        public string kd_kategori { get; set; }

        public string kd_jns_ptg { get; set; }

        public string kd_wilayah { get; set; }

        public short no_kategori { get; set; }

        public decimal nilai_ptg_mul { get; set; }
        public decimal nilai_ptg_akh { get; set; }
        public byte stn_rate_prm { get; set; }
        public decimal pst_rate_prm_min { get; set; }
        public decimal pst_rate_prm_max { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TarifPerluasanKendaraanOJKViewModel, SaveTarifPerluasanKendaraanOJKCommand>();
            profile.CreateMap<TarifPerluasanKendaraanOJKDto, TarifPerluasanKendaraanOJKViewModel>();
        }
    }
}