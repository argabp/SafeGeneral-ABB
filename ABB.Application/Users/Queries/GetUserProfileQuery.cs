using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Services;
using AutoMapper;
using MediatR;

namespace ABB.Application.Users.Queries
{
    public class GetUserProfileQuery : IRequest<UserProfileDto>
    {
    }

    public class GetUserProfileHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto>
    {
        private readonly IUserManagerHelper _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProfilePictureHelper _profilePicturehelper;
        private readonly IMapper _mapper;

        public GetUserProfileHandler(IUserManagerHelper userManager, ICurrentUserService currentUserService,
            IProfilePictureHelper profilePicturehelper, IMapper mapper)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _profilePicturehelper = profilePicturehelper;
            _mapper = mapper;
        }

        public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var User = await _currentUserService.GetUser();
            if (User == null) return new UserProfileDto();
            var role = await _userManager.GetRolesAsync(User);
            var info = _mapper.Map<UserProfileDto>(User);
            info.FullName = $"{User.FirstName} {User.LastName}";
            info.RoleName = role.FirstOrDefault();
            info.Photo = _profilePicturehelper.GetProfilePicture(User.Photo);
            return info;
        }
    }
}