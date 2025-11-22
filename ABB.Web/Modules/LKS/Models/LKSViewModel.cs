using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.LKSs.Commands;
using AutoMapper;

namespace ABB.Web.Modules.LKS.Models
{
    public class LKSViewModel : IMapFrom<SaveLKSCommand>
    {
        public int Id { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public string st_tipe_pla { get; set; }

        public Int16 no_pla { get; set; }

        public string flag_posting { get; set; }

        public string nomor_lks { get; set; }

        public string kd_grp_pas { get; set; }

        public string? kd_rk_pas { get; set; }

        public decimal pst_share { get; set; }

        public decimal nilai_kl { get; set; }

        public string? ket_pla { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LKSViewModel, SaveLKSCommand>();
            profile.CreateMap<Domain.Entities.LKS, LKSViewModel>();
        }
    }
}