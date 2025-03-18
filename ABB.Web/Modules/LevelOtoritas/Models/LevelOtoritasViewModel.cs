using ABB.Application.Common.Interfaces;
using ABB.Application.LevelOtoritass.Commands;
using ABB.Application.LevelOtoritass.Queries;
using AutoMapper;

namespace ABB.Web.Modules.LevelOtoritas.Models
{
    public class LevelOtoritasViewModel : IMapFrom<SaveLevelOtoritasCommand>
    {
        public string kd_user { get; set; }

        public string kd_pass { get; set; }

        public string? flag_xol { get; set; }

        public decimal? nilai_xol { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LevelOtoritasViewModel, SaveLevelOtoritasCommand>();
            profile.CreateMap<LevelOtoritasDto, LevelOtoritasViewModel>();
        }
    }
}