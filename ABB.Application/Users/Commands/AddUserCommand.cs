                                                                                                                                                                         using System;
using System.IO;
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
    public class AddUserCommand : IMapFrom<AppUser>, IRequest
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; } = true;
        public string Photo { get; set; }
        public DateTime PasswordExpiredDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Today;
        public string CreatedBy { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool LockoutEnabled { get; set; } = false;
        public IFormFile ProfilePhoto { get; set; }

        public bool IsDeleted { get; set; }

        public IFormFile SignatureFile { get; set; }

        public string Jabatan { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddUserCommand, AppUser>();
        }
    }

    public class AddUserCommandHandler : IRequestHandler<AddUserCommand>
    {
        private readonly IUserManagerHelper _userManager;
        private readonly IProfilePictureHelper _pictureHelper;
        private readonly IUserHistoryService _userHistoryService;
        private readonly IAuditTrailService _auditService;
        private readonly IMapper _mapper;
        private readonly IDbContext _context;
        private readonly IConfiguration _configuration;

        public AddUserCommandHandler(IUserManagerHelper userManager, IMapper mapper
            , IProfilePictureHelper pictureHelper, IUserHistoryService userHistoryService,
            IAuditTrailService auditService, IDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _pictureHelper = pictureHelper;
            _userHistoryService = userHistoryService;
            _auditService = auditService;
            _context = context;
            _configuration = configuration;
        }

        public async Task<Unit> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<AppUser>(request);
            await AddUser(request, user);
            await AddUserRole(request, user, cancellationToken);
            await AddProfilePicture(request, user);
            await AddSignature(user.Id, request.SignatureFile);
            await AddUserHistory(request, user);
            return Unit.Value;
        }

        private async Task AddUser(AddUserCommand request, AppUser user)
        {
            user.CreatedDate = DateTime.Now;
            user.CreatedBy = request.CreatedBy;
            user.PasswordExpiredDate = DateTime.Now.AddYears(100);
            user.Photo = request.ProfilePhoto == null ? "default-profile-picture.png" : request.ProfilePhoto.FileName;
            user.Signature = request.SignatureFile == null ? string.Empty : request.SignatureFile.FileName;
            user.Id ??= Guid.NewGuid().ToString("N");
            user.LockoutEnabled = false;
            user.Jabatan = request.Jabatan;
            var result = await _userManager.CreateAsync(user, request.Password);

            await _auditService.Create(new AuditTrailDto()
            {
                AuditEvent = AuditEvent.UserCreation,
                Status = result ? AuditStatus.Success : AuditStatus.Failed,
                Platform = AuditPlatform.Web,
                UserId = request.CreatedBy
            });
        }

        private async Task AddUserHistory(AddUserCommand request, AppUser user)
        {
            var userhistory = _mapper.Map<UserHistoryDto>(user);
            userhistory.Password = Encryption.Encrypt(request.Password);
            _userHistoryService.AddUserHistory(userhistory);
            await _userHistoryService.Commit();
        }


        private async Task AddUserRole(AddUserCommand request, AppUser user, CancellationToken cancellationToken)
        {
            var userRole = new UserRole()
            {
                UserId = user.Id,
                RoleId = request.RoleId
            };
            _context.UserRole.Add(userRole);

            var result = await _context.SaveChangesAsync(cancellationToken);

            await _auditService.Create(new AuditTrailDto()
            {
                AuditEvent = AuditEvent.RoleCreation,
                Status = result == 1 ? AuditStatus.Success : AuditStatus.Failed,
                Platform = AuditPlatform.Web,
                UserId = request.CreatedBy
            });
        }

        private async Task AddProfilePicture(AddUserCommand request, AppUser user)
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