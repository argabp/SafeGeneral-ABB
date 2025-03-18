using System.Collections.Generic;
using ABB.Application.Common.Interfaces;
using ABB.Application.Navigations.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Navigation.Models
{
    public class AddNavigationModel : IMapFrom<AddNavigationWithSubnavigationCommand>
    {
        public AddNavigationModel()
        {
        }

        public string Text { get; set; }
        public int? RouteId { get; set; }
        public bool IsActive { get; set; }
        public string Icon { get; set; }
        public List<SubNavigation> SubNavigations { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddNavigationModel, AddNavigationWithSubnavigationCommand>();
        }
    }
}