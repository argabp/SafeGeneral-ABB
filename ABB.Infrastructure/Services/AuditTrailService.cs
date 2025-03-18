using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Domain.Entities;
using ABB.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace ABB.Infrastructure.Services
{
    public class AuditTrailService : IAuditTrailService
    {
        private readonly IDbContext _context;
        private readonly IHttpContextAccessor _http;

        public AuditTrailService(IDbContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }

        public async Task Create(AuditTrailDto model)
        {
            if (model.UserId == null) return;
            var item = new AuditTrail()
            {
                AuditEvent = Enum.GetName(typeof(AuditEvent), model.AuditEvent),
                Date = DateTime.Now,
                Platform = Enum.GetName(typeof(AuditPlatform), model.Platform),
                Status = Enum.GetName(typeof(AuditStatus), model.Status),
                ClientSource = _http.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                UserId = model.UserId
            };
            await _context.AuditTrail.AddAsync(item);
            await _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}