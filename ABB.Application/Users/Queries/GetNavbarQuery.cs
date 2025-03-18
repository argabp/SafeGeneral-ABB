using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Services;
using MediatR;

namespace ABB.Application.Users.Queries
{
    public class GetNavbarQuery : IRequest<NavbarDto>
    {
    }

    public class GetSidebarInfoHandler : IRequestHandler<GetNavbarQuery, NavbarDto>
    {
        private readonly IUserManagerHelper _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProfilePictureHelper _profilePictureHelper;

        public GetSidebarInfoHandler(IUserManagerHelper userManager, ICurrentUserService currentUserService,
            IProfilePictureHelper profilePictureHelper)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _profilePictureHelper = profilePictureHelper;
        }

        public async Task<NavbarDto> Handle(GetNavbarQuery request, CancellationToken cancellationToken)
        {
            var User = await _currentUserService.GetUser();
            if (User == null) return new NavbarDto();
            var role = await _userManager.GetRolesAsync(User);
            var info = new NavbarDto()
            {
                FullName = $"{User.FirstName} {User.LastName}",
                Id = User.Id,
                PhoneNumber = User.PhoneNumber,
                RoleName = role.FirstOrDefault(),
                UserName = User.UserName,
            };
            info.Photo = _profilePictureHelper.GetProfilePicture(User.Photo);
            return info;
        }
    }
}