using System;
using System.Collections.Generic;
using ABB.Application.Common.Interfaces;
using ABB.Application.KontrakTreatyKeluarXOLs.Queries;
using AutoMapper;

namespace ABB.Web.Modules.KontrakTreatyKeluarXOL.Models
{
    public class DetailKontrakTreatyKeluarXOLViewModel : IMapFrom<DetailKontrakTreatyKeluarXOLDataDto>
    {
        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }

        public decimal pst_share { get; set; }

        public decimal pst_com { get; set; }

        public string? kd_grp_sb_bis { get; set; }

        public string? kd_rk_sb_bis { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DetailKontrakTreatyKeluarXOLViewModel, DetailKontrakTreatyKeluarXOLDataDto>();
        }
    }
}