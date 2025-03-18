using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Services;
using ABB.Application.Users.Queries;
using ABB.Domain.Enums;
using ABB.Domain.IdentityModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ABB.Application.Users.Commands
{
    public class ChangeUserProfileCommand : IRequest
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Today;
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public IFormFile ProfilePhoto { get; set; }
    }

    public class ChangeUserProfileCommandHandler : IRequestHandler<ChangeUserProfileCommand>
    {
        private readonly IUserManagerHelper _userManager;
        private readonly IProfilePictureHelper _pictureHelper;
        private readonly IUserHistoryService _userHistoryService;
        private readonly IAuditTrailService _auditService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public ChangeUserProfileCommandHandler(IUserManagerHelper userManager, IProfilePictureHelper pictureHelper
            , IMapper mapper, IUserHistoryService userHistoryService, IAuditTrailService auditService,
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _pictureHelper = pictureHelper;
            _mapper = mapper;
            _userHistoryService = userHistoryService;
            _auditService = auditService;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(ChangeUserProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            var result = await UpdateUser(request, user);
            await AddProfilePicture(request, user);
            await AddUserHistory(user);
            await AddAuditTrail(request, result);
            return Unit.Value;
        }

        private async Task<bool> UpdateUser(ChangeUserProfileCommand request, AppUser user)
        {
            user.UpdatedDate = DateTime.Now;
            user.UpdatedBy = _currentUserService.UserId;
            user.Photo = user.Photo ?? "default-profile-picture.png";
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            user.Address = request.Address;
            return await _userManager.UpdateAsync(user);
        }

        public async Task AddProfilePicture(ChangeUserProfileCommand request, AppUser user)
        {
            if (request.ProfilePhoto == null) return;
            user.Photo = await _pictureHelper.Upload(request.ProfilePhoto);
            await _userManager.UpdateAsync(user);
        }

        private async Task AddUserHistory(AppUser user)
        {
            var userhistory = _mapper.Map<UserHistoryDto>(user);
            _userHistoryService.AddUserHistory(userhistory);
            await _userHistoryService.Commit();
        }

        private async Task AddAuditTrail(ChangeUserProfileCommand request, bool result)
        {
            await _auditService.Create(new AuditTrailDto()
            {
                AuditEvent = AuditEvent.UserModification,
                Platform = AuditPlatform.Web,
                Status = result ? AuditStatus.Success : AuditStatus.Failed,
                UserId = request.Id
            });
        }
    }
}