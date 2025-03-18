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

namespace ABB.Application.Users.Commands
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IUserManagerHelper _userManager;
        private readonly IUserHistoryService _userHistoryService;
        private readonly IAuditTrailService _auditService;
        private readonly IMapper _mapper;

        public ChangePasswordCommandHandler(IUserManagerHelper userManager, IMapper mapper
            , IUserHistoryService userHistoryService, IAuditTrailService auditService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _userHistoryService = userHistoryService;
            _auditService = auditService;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            var result = await ResetPassword(request, user);
            await AddUserHistory(request, user);
            return result;
        }

        private async Task<bool> ResetPassword(ChangePasswordCommand request, AppUser user)
        {
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            user.PasswordExpiredDate = DateTime.Now.AddDays(90);
            user.UpdatedBy = request.UpdatedBy;
            user.UpdatedDate = DateTime.Now;
            var result = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

            await AddAuditTrail(request, result);
            return result;
        }

        private async Task AddUserHistory(ChangePasswordCommand request, AppUser user)
        {
            var userhistory = _mapper.Map<UserHistoryDto>(user);
            userhistory.Password = Encryption.Encrypt(request.NewPassword);
            _userHistoryService.AddUserHistory(userhistory);
            await _userHistoryService.Commit();
        }

        private async Task AddAuditTrail(ChangePasswordCommand request, bool result)
        {
            await _auditService.Create(new AuditTrailDto()
            {
                AuditEvent = AuditEvent.PasswordReset,
                Platform = AuditPlatform.Web,
                Status = result ? AuditStatus.Success : AuditStatus.Failed,
                UserId = request.Id
            });
        }
    }
}