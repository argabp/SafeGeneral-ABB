
using DinkToPdf;

namespace ABB.Application.Common.Services
{
    public interface IReportGeneratorService
    {
        public void GenerateReport(string reportName, string templateReport, string path, 
            Orientation orientation = Orientation.Portrait, double right = 20, double left = 20);
    }
}