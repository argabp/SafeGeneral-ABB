using System;

namespace ABB.Domain.Entities
{
    public class ApiAuditTrail
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public int Status { get; set; }

        public string Keterangan { get; set; }

        public string Data { get; set; }

        public string Response { get; set; }
        
        public DateTime Date { get; set; }
    }
}