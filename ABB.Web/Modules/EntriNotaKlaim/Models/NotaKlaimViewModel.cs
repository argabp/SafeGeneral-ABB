using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.EntriNotaKlaims.Commands;
using AutoMapper;

namespace ABB.Web.Modules.EntriNotaKlaim.Models
{
    public class NotaKlaimViewModel : IMapFrom<SaveEntriNotaKlaimCommand>
    {
        public string kd_cb { get; set; }

        public string jns_tr { get; set; }

        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }

        public string? kd_grp_ttj { get; set; }

        public string? kd_rk_ttj { get; set; }

        public string? nm_ttj { get; set; }

        public string? almt_ttj { get; set; }

        public string? kt_ttj { get; set; }

        public string? ket_nt { get; set; }

        public string? ket_kwi { get; set; }

        public decimal nilai_nt { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }
        public string no_kl { get; set; }
        public Int16 no_mts { get; set; }
        public Int16 no_dla { get; set; }

        public string st_tipe_dla { get; set; }

        public string kd_mtu { get; set; }

        public string tipe_mts { get; set; }
        public double nilai_bia_mat { get; set; }
        public DateTime tgl_nt { get; set; }
        public string flag_posting { get; set; }
        public string flag_cancel { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NotaKlaimViewModel, SaveEntriNotaKlaimCommand>();
            profile.CreateMap<Domain.Entities.EntriNotaKlaim, NotaKlaimViewModel>();
        }
    }
}