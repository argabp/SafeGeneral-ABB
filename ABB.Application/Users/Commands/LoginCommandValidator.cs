using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Domain.Enums;
using FluentValidation;

namespace ABB.Application.Users.Commands
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        private readonly IUserManagerHelper _userManager;
        private readonly ILockOutService _lockOutService;
        private readonly IAuditTrailService _auditService;
        private readonly IDbContext _dbContext;

        public LoginCommandValidator(IUserManagerHelper userManager
            , ILockOutService lockOutService, IAuditTrailService auditService, IDbContext dbContext)
        {
            _userManager = userManager;
            _lockOutService = lockOutService;
            _auditService = auditService;
            _dbContext = dbContext;

            RuleFor(p => p.Username)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Username field is required")
                .Must(UserExist).WithMessage("Invalid Username")
                .MustAsync(WaitMatchUsernamePassword).WithMessage(" ")
                .Must(UserActive).WithMessage("Account is not active, please contact your Administrator");
            RuleFor(p => p.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Password field is required")
                .Must(NotUserLockOut)
                .WithMessage("Your account login have been locked out, please contact your Administrator")
                .MustAsync(MatchUsernamePassword).WithMessage("Invalid Password")
                .Must(NotExpiredPassword).WithMessage("Your password has expired, change your password");
            RuleFor(p => p.UserDatabase)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Cabang field is required");
        }

        private bool UserExist(string username)
        {
            var result = _dbContext.User.Any(a => a.UserName == username && !a.IsDeleted);
            return result;
        }

        private bool UserActive(string username)
        {
            return _dbContext.User.FirstOrDefault(a => a.UserName == username).IsActive;
        }

        private bool NotUserLockOut(LoginCommand model, string password)
        {
            return !(_dbContext.User.FirstOrDefault(a => a.UserName == model.Username)?.LockoutEnabled ?? false);
        }

        private async Task<bool> MatchUsernamePassword(LoginCommand model, string username, CancellationToken token)
        {
            if (model.Password == null) return false;
            var user = _dbContext.User.FirstOrDefault(a => a.UserName == model.Username && !a.IsDeleted);
            var validPassword = await _userManager.CheckPasswordAsync(user, model.Password);
            await _lockOutService.UpdateAccessFailedCount(user, validPassword);
            await _lockOutService.CheckingLockOut(user);
            await _auditService.Create(new AuditTrailDto()
            {
                AuditEvent = AuditEvent.LoginAttempt,
                Status = validPassword ? AuditStatus.Success : AuditStatus.Failed,
                Platform = AuditPlatform.Web,
                UserId = user?.Id
            });
            return validPassword;
        }

        private async Task<bool> WaitMatchUsernamePassword(LoginCommand model, string username, CancellationToken token)
        {
            if (model.Password == null) return false;
            var user = _dbContext.User.FirstOrDefault(a => a.UserName == model.Username);
            return await _userManager.CheckPasswordAsync(user, model.Password);
        }

        private bool NotExpiredPassword(LoginCommand model, string password)
        {
            var user = _dbContext.User.FirstOrDefault(a => a.UserName == model.Username);
            return user.PasswordExpiredDate > DateTime.Now;
        }
    }
}