using System;

namespace ABB.Domain.Entities
{
    public class AuditTrail
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public string AuditEvent { get; set; }
        public string Platform { get; set; }
        public string Status { get; set; }
        public string ClientSource { get; set; }
    }
}