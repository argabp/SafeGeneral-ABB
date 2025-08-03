using System;
using ABB.Application.Akseptasis.Commands;
using ABB.Application.Akseptasis.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Akseptasi.Models
{
    public class AkseptasiResikoOtherBondingViewModel : IMapFrom<AkseptasiOtherBondingDto>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public string kd_grp_prc { get; set; }

        public string? kd_rk_prc { get; set; }

        public string grp_obl { get; set; }

        public string kd_obl { get; set; }

        public string grp_kontr { get; set; }

        public string kd_kontr { get; set; }

        public decimal nilai_bond { get; set; }

        public string nm_obl { get; set; }

        public string? almt_obl { get; set; }

        public string? kt_obl { get; set; }

        public string kd_rumus { get; set; }

        public string? nm_kons { get; set; }

        public decimal? nilai_kontr { get; set; }

        public string? ket_rincian_kontr { get; set; }

        public DateTime? tgl_terbit { get; set; }

        public DateTime? tgl_lelang { get; set; }

        public DateTime? tgl_tr { get; set; }

        public string? no_pol_ttg { get; set; }

        public string? nm_principal { get; set; }

        public string? jbt_principal { get; set; }

        public DateTime? tgl_kontrak { get; set; }

        public string? tmpt_lelang { get; set; }

        public string? ba_serah_trm { get; set; }

        public string grp_jns_pekerjaan { get; set; }

        public string jns_pekerjaan { get; set; }

        public string? kd_rk_obl { get; set; }

        public string kd_grp_obl { get; set; }

        public string kd_grp_surety { get; set; }

        public string kd_rk_surety { get; set; }

        public string kd_bag { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AkseptasiOtherBondingDto, AkseptasiResikoOtherBondingViewModel>();
            profile.CreateMap<AkseptasiResikoOtherBondingViewModel, SaveAkseptasiOtherBondingCommand>();
        }
    }
}