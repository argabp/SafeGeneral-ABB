using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.LKTs.Commands;
using AutoMapper;

namespace ABB.Web.Modules.LKT.Models
{
    public class LKTViewModel : IMapFrom<SaveLKTCommand>
    {
        public int Id { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public string st_tipe_dla { get; set; }

        public Int16 no_dla { get; set; }

        public string nomor_lkt { get; set; }

        public string kd_grp_pas { get; set; }

        public string? kd_rk_pas { get; set; }

        public decimal pst_share { get; set; }

        public decimal nilai_kl { get; set; }

        public string? ket_dla { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LKTViewModel, SaveLKTCommand>();
            profile.CreateMap<Domain.Entities.LKT, LKTViewModel>();
        }
    }
}