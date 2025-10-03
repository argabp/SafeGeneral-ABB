
using DinkToPdf;

namespace ABB.Application.Common.Services
{
    public interface IReportGeneratorService
    {
        public void GenerateReport(string reportName, string templateReport, string path, 
            Orientation orientation = Orientation.Portrait, double right = 20, double left = 20,
            double top = 20, double bottom = 20, PaperKind paperSize = PaperKind.A4);
    }
}