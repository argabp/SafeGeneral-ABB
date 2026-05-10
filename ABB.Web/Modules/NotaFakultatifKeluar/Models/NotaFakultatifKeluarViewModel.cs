using System;
using System.Collections.Generic;
using ABB.Application.Common.Interfaces;
using ABB.Application.NotaFakultatifKeluars.Commands;
using ABB.Application.NotaFakultatifKeluars.Queries;
using AutoMapper;

namespace ABB.Web.Modules.NotaFakultatifKeluar.Models
{
    public class NotaFakultatifKeluarViewModel : IMapFrom<SaveNotaFakultatifKeluarCommand>
    {
        public string kd_cb { get; set; }

        public string jns_tr { get; set; }
        
        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public short no_rsk { get; set; }

        public string kd_endt { get; set; }

        public short no_updt_reas { get; set; }

        public string kd_mtu { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }

        public string? ket_nt { get; set; }

        public decimal nilai_nt { get; set; }

        public DateTime tgl_nt { get; set; }

        public string flag_cancel { get; set; }

        public string flag_posting { get; set; }

        public string? kd_cb_pol { get; set; }

        public byte? wpc { get; set; }

        public DateTime? tgl_mul_reas { get; set; }

        public DateTime? tgl_akh_reas { get; set; }

        public string? no_pol_ttg { get; set; }

        public string? ket_tc { get; set; }

        public string? no_slip { get; set; }

        public string? kd_grp_sb_bis { get; set; }

        public string? kd_rk_sb_bis { get; set; }

        public string? flag_nd { get; set; }

        public List<DetailNotaFakultatifKeluarDto> Details { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NotaFakultatifKeluarViewModel, SaveNotaFakultatifKeluarCommand>();
            profile.CreateMap<Domain.Entities.NotaFakultatifKeluar, NotaFakultatifKeluarViewModel>();
        }
    }
}