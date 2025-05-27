using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Application.Users.Queries;
using ABB.Domain.Entities;
using ABB.Domain.Enums;
using ABB.Domain.IdentityModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ABB.Application.Users.Commands
{
    public class EditUserCommand : IRequest
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;
        public bool LockoutEnabled { get; set; } = false;
        public string Photo { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Today;
        public string UpdatedBy { get; set; }
        public string LeaderId { get; set; } = "";
        public string RoleId { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public string RoleName { get; set; }
        public IFormFile ProfilePhoto { get; set; }
        public IFormFile SignatureFile { get; set; }

        public string Jabatan { get; set; }
    }

    public class EditUserCommandHandler : IRequestHandler<EditUserCommand>
    {
        private readonly IUserManagerHelper _userManager;
        private readonly IProfilePictureHelper _pictureHelper;
        private readonly IUserHistoryService _userHistoryService;
        private readonly IAuditTrailService _auditService;
        private readonly IMapper _mapper;
        private readonly IDbContext _context;
        private readonly IConfiguration _configuration;

        public EditUserCommandHandler(IUserManagerHelper userManager, IProfilePictureHelper pictureHelper
            , IMapper mapper, IUserHistoryService userHistoryService, IAuditTrailService auditService,
            IDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _pictureHelper = pictureHelper;
            _mapper = mapper;
            _userHistoryService = userHistoryService;
            _auditService = auditService;
            _context = context;
            _configuration = configuration;
        }

        public async Task<Unit> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            await UpdateUser(request, user);
            await UpdateRole(request, user, cancellationToken);
            await AddProfilePicture(request, user);
            await AddUserHistory(user);
            return Unit.Value;
        }

        private async Task UpdateUser(EditUserCommand request, AppUser user)
        {
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.Address = request.Address;
            user.IsActive = request.IsActive;
            user.LockoutEnabled = request.LockoutEnabled;
            user.UpdatedDate = DateTime.Now;
            user.UpdatedBy = request.UpdatedBy;
            user.Photo = request.ProfilePhoto == null ? string.Empty : request.ProfilePhoto.Name;
            user.Signature = request.SignatureFile == null ? string.Empty : request.SignatureFile.Name;
            user.IsActive = request.IsActive;

            var result = await _userManager.UpdateAsync(user);
            await _auditService.Create(new AuditTrailDto()
            {
                AuditEvent = AuditEvent.UserModification,
                Status = result ? AuditStatus.Success : AuditStatus.Failed,
                Platform = AuditPlatform.Web,
                UserId = request.UpdatedBy
            });
        }

        private async Task AddUserHistory(AppUser user)
        {
            var userhistory = _mapper.Map<UserHistoryDto>(user);
            _userHistoryService.AddUserHistory(userhistory);
            await _userHistoryService.Commit();
        }

        private async Task UpdateRole(EditUserCommand request, AppUser user, CancellationToken cancellationToken)
        {
            var userRole = await _userManager.GetRolesAsync(user);
            if (request.RoleName != userRole.FirstOrDefault())
            {
                await _userManager.RemoveFromRolesAsync(user, userRole);
                var userRoles = new UserRole()
                {
                    UserId = user.Id,
                    RoleId = request.RoleId
                };
                _context.UserRole.Add(userRoles);

                var result = await _context.SaveChangesAsync(cancellationToken);

                await _auditService.Create(new AuditTrailDto()
                {
                    AuditEvent = AuditEvent.RoleModification,
                    Status = result == 1 ? AuditStatus.Success : AuditStatus.Failed,
                    Platform = AuditPlatform.Web,
                    UserId = request.UpdatedBy
                });
            }
        }

        private async Task AddProfilePicture(EditUserCommand request, AppUser user)
        {
            if (request.ProfilePhoto == null) return;

            user.Photo = await _pictureHelper.Upload(request.ProfilePhoto);

            await _userManager.UpdateAsync(user);
        }

        private async Task AddSignature(string userId, IFormFile file)
        {
            var path = _configuration.GetSection("UserSignature").Value.TrimEnd('/');
            path = Path.Combine(path, userId);
                
            await _pictureHelper.UploadToFolder(file, path);
        }
    }
}