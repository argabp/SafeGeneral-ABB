using ABB.Domain.Enums;

namespace ABB.Application.Common.Dtos
{
    public class AuditTrailDto
    {
        public string UserId { get; set; }
        public AuditEvent AuditEvent { get; set; }
        public AuditPlatform Platform { get; set; }
        public AuditStatus Status { get; set; }
    }
}