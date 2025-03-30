using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.EntriNotas.Commands;
using ABB.Application.EntriNotas.Queries;
using AutoMapper;

namespace ABB.Web.Modules.EntriNota.Models
{
    public class DetailNotaViewModel : IMapFrom<DetailNotaDto>
    {
        public string kd_cb { get; set; }

        public string jns_tr { get; set; }

        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }

        public byte no_ang { get; set; }

        public DateTime tgl_ang { get; set; }

        public DateTime tgl_jth_tempo { get; set; }

        public decimal pst_ang { get; set; }

        public decimal nilai_ang { get; set; }
    }
}