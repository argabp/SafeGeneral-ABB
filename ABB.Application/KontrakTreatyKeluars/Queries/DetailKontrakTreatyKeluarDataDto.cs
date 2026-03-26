using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.KontrakTreatyKeluars.Queries
{
    public class DetailKontrakTreatyKeluarDataDto : IMapFrom<DetailKontrakTreatyKeluar>
    {
        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }

        public decimal pst_share { get; set; }

        public decimal pst_com { get; set; }

        public string? kd_grp_sb_bis { get; set; }

        public string? kd_rk_sb_bis { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DetailKontrakTreatyKeluarDataDto, DetailKontrakTreatyKeluar>();
        }
    }
}