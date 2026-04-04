using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.NotaTreatyMasukXOLs.Commands;
using AutoMapper;

namespace ABB.Web.Modules.NotaTreatyMasukXOL.Models
{
    public class NotaTreatyMasukXOLViewModel : IMapFrom<SaveNotaTreatyMasukXOLCommand>
    {
        public string kd_cb { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_msk { get; set; }

        public string kd_mtu { get; set; }

        public string no_tr { get; set; }

        public DateTime tgl_closing { get; set; }

        public decimal thn_tr { get; set; }

        public byte? kuartal_tr { get; set; }

        public string? ket_tr { get; set; }

        public decimal nilai_ttl_ptg { get; set; }

        public decimal nilai_prm { get; set; }

        public decimal nilai_kms { get; set; }

        public decimal nilai_kl { get; set; }

        public string? nm_ttg { get; set; }

        public string kd_usr_input { get; set; }

        public DateTime tgl_input { get; set; }

        public string? kd_usr_updt { get; set; }

        public DateTime? tgl_updt { get; set; }

        public string? kd_usr_closing { get; set; }

        public string flag_closing { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NotaTreatyMasukXOLViewModel, SaveNotaTreatyMasukXOLCommand>();
            profile.CreateMap<Domain.Entities.TransaksiTreatyMasuk, NotaTreatyMasukXOLViewModel>();
        }
    }
}