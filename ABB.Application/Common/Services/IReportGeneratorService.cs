
using DinkToPdf;

namespace ABB.Application.Common.Services
{
    public interface IReportGeneratorService
    {
        public void GenerateReport(string reportName, string templateReport, string path, Orientation orientation = Orientation.Portrait);
    }
}