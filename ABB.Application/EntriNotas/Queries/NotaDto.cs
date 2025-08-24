using System;
using System.Collections.Generic;
using ABB.Application.Common.Interfaces;
using ABB.Application.EntriNotas.Commands;
using AutoMapper;

namespace ABB.Application.EntriNotas.Queries
{
    public class NotaDto : IMapFrom<SaveEntriNotaCommand>
    {
        public NotaDto()
        {
            Details = new List<DetailNotaDto>();
        }
        
        public int Id { get; set; }
        
        public string kd_cb { get; set; }

        public string nm_cb { get; set; }

        public string jns_tr { get; set; }

        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }

        public string kd_cob { get; set; }

        public string nm_cob { get; set; }

        public string kd_scob { get; set; }
        
        public string nm_scob { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public string kd_mtu { get; set; }

        public string? kd_grp_sb_bis { get; set; }

        public string? kd_rk_sb_bis { get; set; }

        public string? kd_grp_ttj { get; set; }

        public string? kd_rk_ttj { get; set; }

        public string? nm_ttj { get; set; }

        public string? almt_ttj { get; set; }

        public string? kt_ttj { get; set; }

        public string? ket_nt { get; set; }
        
        public string? ket_kwi { get; set; }

        public decimal nilai_nt { get; set; }

        public DateTime tgl_nt { get; set; }

        public string flag_cancel { get; set; }

        public string flag_posting { get; set; }

        public decimal pst_ppn { get; set; }

        public decimal? nilai_ppn { get; set; }

        public decimal pst_pph { get; set; }

        public decimal? nilai_pph { get; set; }

        public string? no_pol_ttg { get; set; }

        public decimal? pst_lain { get; set; }

        public decimal? nilai_lain { get; set; }

        public List<DetailNotaDto> Details { get; set; }

        public string? bayar { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NotaDto, SaveEntriNotaCommand>();
            profile.CreateMap<NotaDto, SaveEntriNotaCancelCommand>();
        }
    }
}