using ABB.Application.Common.Interfaces;
using ABB.Application.TarifKendaraanOJKs.Commands;
using ABB.Application.TarifKendaraanOJKs.Queries;
using AutoMapper;

namespace ABB.Web.Modules.TarifKendaraanOJK.Models
{
    public class TarifKendaraanOJKViewModel : IMapFrom<SaveTarifKendaraanOJKCommand>
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
            profile.CreateMap<TarifKendaraanOJKViewModel, SaveTarifKendaraanOJKCommand>();
            profile.CreateMap<TarifKendaraanOJKDto, TarifKendaraanOJKViewModel>();
        }
    }
}