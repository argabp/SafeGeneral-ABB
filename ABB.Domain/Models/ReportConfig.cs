using System.Collections.Generic;

namespace ABB.Domain.Models
{
    public class ReportConfig
    {
        public List<ReportData> Configurations { get; set; }
    }

    public class ReportData
    {
        public string Database { get; set; }

        public Title Title { get; set; }

        public string NamaPejabat { get; set; }

        public string Jabatan { get; set; }

        public string NamaTTDFile { get; set; }
    }

    public class Title
    {
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Title3 { get; set; }
        public string Title4 { get; set; }
        public string Title5 { get; set; }
        public string Title6 { get; set; }
        public string Title7 { get; set; }
        public string Title8 { get; set; }
    }
}