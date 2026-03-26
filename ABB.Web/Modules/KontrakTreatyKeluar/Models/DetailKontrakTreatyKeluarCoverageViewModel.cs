using ABB.Application.Common.Interfaces;
using ABB.Application.KontrakTreatyKeluars.Commands;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Web.Modules.KontrakTreatyKeluar.Models
{
    public class DetailKontrakTreatyKeluarCoverageViewModel : IMapFrom<SaveDetailKontrakTreatyKeluarCoverageCommand>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public string kd_cvrg { get; set; }

        public decimal pst_kms_reas { get; set; }

        public decimal? max_limit_jktb { get; set; }
        
        public decimal? max_limit_prov { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DetailKontrakTreatyKeluarCoverage, DetailKontrakTreatyKeluarCoverageViewModel>();
            profile.CreateMap<DetailKontrakTreatyKeluarCoverageViewModel, SaveDetailKontrakTreatyKeluarCoverageCommand>();
        }
    }
}