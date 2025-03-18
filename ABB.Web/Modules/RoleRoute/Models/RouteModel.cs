using ABB.Application.Common.Interfaces;
using ABB.Application.RoleRoutes.Queries;
using AutoMapper;

namespace ABB.Web.Modules.RoleRoute.Models
{
    public class RouteModel : IMapFrom<RoutesDto>
    {
        public int RouteId { get; set; }

        public string Action { get; set; }

        public string Controller { get; set; }

        public bool Active { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RouteModel, RoutesDto>();
            profile.CreateMap<RoutesDto, RouteModel>();
        }
    }
}