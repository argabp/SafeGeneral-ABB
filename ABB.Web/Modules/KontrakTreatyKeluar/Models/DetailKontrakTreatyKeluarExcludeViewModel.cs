using ABB.Application.Common.Interfaces;
using ABB.Application.KontrakTreatyKeluars.Commands;
using ABB.Application.KontrakTreatyKeluars.Queries;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Web.Modules.KontrakTreatyKeluar.Models
{
    public class DetailKontrakTreatyKeluarExcludeViewModel : IMapFrom<SaveDetailKontrakTreatyKeluarExcludeCommand>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public string kd_okup { get; set; }

        public string? kd_cvrg { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DetailKontrakTreatyKeluarExclude, DetailKontrakTreatyKeluarExcludeViewModel>();
            profile.CreateMap<DetailKontrakTreatyKeluarExcludeViewModel, SaveDetailKontrakTreatyKeluarExcludeCommand>();
        }
    }
}