using System.Collections.Generic;
using ABB.Application.Common.Interfaces;
using ABB.Application.Navigations.Commands;
using ABB.Application.Navigations.Queries;
using AutoMapper;

namespace ABB.Web.Modules.Navigation.Models
{
    public class EditNavigationModel : IMapFrom<EditNavigationWithSubnavigationCommand>, IMapFrom<GetEditNavigationDto>
    {
        public EditNavigationModel()
        {
        }

        public int NavigationId { get; set; }
        public string Text { get; set; }
        public int? RouteId { get; set; }
        public bool IsActive { get; set; }
        public string Icon { get; set; }
        public List<SubNavigation> SubNavigations { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GetEditNavigationDto, EditNavigationModel>();
            profile.CreateMap<EditNavigationModel, EditNavigationWithSubnavigationCommand>();
        }
    }
}