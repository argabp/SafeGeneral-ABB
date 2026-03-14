using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.KontrakTreatyMasuks.Commands;
using AutoMapper;

namespace ABB.Web.Modules.KontrakTreatyMasuk.Models
{
    public class KontrakTreatyMasukViewModel : IMapFrom<SaveKontrakTreatyMasukCommand>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_msk { get; set; }

        public string tipe_tty_msk { get; set; }

        public string kd_cob { get; set; }

        public decimal thn_uw { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }

        public string? desk_tty { get; set; }

        public decimal pst_share_tty { get; set; }

        public DateTime? tgl_mul_ptg { get; set; }

        public DateTime? tgl_akh_ptg { get; set; }

        public string? kd_grp_sb_bis { get; set; }

        public string? kd_rk_sb_bis { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<KontrakTreatyMasukViewModel, SaveKontrakTreatyMasukCommand>();
            profile.CreateMap<Domain.Entities.KontrakTreatyMasuk, KontrakTreatyMasukViewModel>();
        }
    }
}