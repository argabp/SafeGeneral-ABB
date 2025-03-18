using ABB.Application.Common.Interfaces;
using ABB.Application.Coverages.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Coverage.Models
{
    public class CoverageViewModel : IMapFrom<SaveCoverageCommand>
    {
        public string kd_cvrg { get; set; }
        public string nm_cvrg { get; set; }
        public string nm_cvrg_ing { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CoverageViewModel, SaveCoverageCommand>();
            profile.CreateMap<CoverageViewModel, DeleteCoverageCommand>();
        }
    }
}