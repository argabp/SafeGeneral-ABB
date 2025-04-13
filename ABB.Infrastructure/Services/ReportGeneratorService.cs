using System;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Services;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace ABB.Infrastructure.Services
{
    public class ReportGeneratorService : IReportGeneratorService
    {
        private readonly IConverter _converter;
        private readonly IProfilePictureHelper _profilePictureHelper;

        public ReportGeneratorService(IConverter converter, IProfilePictureHelper profilePictureHelper)
        {
            _converter = converter;
            _profilePictureHelper = profilePictureHelper;
        }

        public void GenerateReport(string reportName, string templateReport, string path, Orientation orientation = Orientation.Portrait)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = orientation,
                    PaperSize = PaperKind.A4,
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = templateReport,
                        WebSettings = { DefaultEncoding = "utf-8" },
                    }
                }
            };

            var bytes = _converter.Convert(doc);

            _profilePictureHelper.UploadByteFile(bytes, reportName, path);
        }
    }
}