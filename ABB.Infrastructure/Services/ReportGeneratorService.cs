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

        public void GenerateReport(string reportName, string templateReport, string path, 
            Orientation orientation = Orientation.Portrait, double right = 20, double left = 20, 
            double top = 20, double bottom = 20, PaperKind paperSize = PaperKind.A4, bool useFooter = false)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = orientation,
                    PaperSize = paperSize,
                    Margins = new MarginSettings { Top = top, Bottom = bottom, Left = left, Right = right }
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = templateReport,
                        WebSettings = { DefaultEncoding = "utf-8", EnableIntelligentShrinking = false },
                        FooterSettings = useFooter ? new FooterSettings
                        {
                        Center = "Test",   // or Page numbering
                        FontSize = 9,
                        Spacing = 5
                        } : null
                    }
                }
            };

            var bytes = _converter.Convert(doc);

            _profilePictureHelper.UploadByteFile(bytes, reportName, path);
        }
    }
}