using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Domain.Enums;
using ABB.Domain.IdentityModels;
using Microsoft.Extensions.Configuration;

namespace ABB.Infrastructure.Services
{
    public class LockOutService : ILockOutService
    {
        private readonly IDbContext _context;
        private readonly IConfiguration _config;
        private readonly IAuditTrailService _auditService;

        public LockOutService(IDbContext context, IConfiguration config, IAuditTrailService auditService)
        {
            _context = context;
            _config = config;
            _auditService = auditService;
        }

        public async Task CheckingLockOut(AppUser user)
        {
            if (user == null) return;
            var maxAccessFailedCount = Convert.ToInt32(_config.GetSection("MaxAccessFailedCount").Value);
            if (user.AccessFailedCount >= maxAccessFailedCount)
            {
                user.LockoutEnabled = true;
                await _context.SaveChangesAsync(CancellationToken.None);
                await _auditService.Create(new AuditTrailDto()
                {
                    AuditEvent = AuditEvent.LockOut,
                    Status = AuditStatus.Success,
                    Platform = AuditPlatform.Web,
                    UserId = user?.Id
                });
            }
        }

        public async Task UpdateAccessFailedCount(AppUser user, bool validPassword)
        {
            if (user == null) return;
            user.AccessFailedCount = validPassword ? 0 : user.AccessFailedCount + 1;
            await _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}