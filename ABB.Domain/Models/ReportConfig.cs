using System.Collections.Generic;
using System.Linq;

namespace ABB.Domain.Models
{
    public class ReportConfig
    {
        public List<ReportData> Configurations { get; set; }

        public ReportData GetReportData(string kodeCabang)
        {
            return Configurations.FirstOrDefault(w => w.KodeCabang.Trim() == kodeCabang.Trim()) ?? new ReportData();
        }
    }

    public class ReportData
    {
        public string KodeCabang { get; set; }

        public Title Title { get; set; }
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