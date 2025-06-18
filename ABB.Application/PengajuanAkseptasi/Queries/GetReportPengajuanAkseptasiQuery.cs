using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Scriban;

namespace ABB.Application.PengajuanAkseptasi.Queries
{
    public class GetReportPengajuanAkseptasiQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_aks { get; set; }
    }

    public class GetReportPengajuanAkseptasiQueryHandler : IRequestHandler<GetReportPengajuanAkseptasiQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;

        public GetReportPengajuanAkseptasiQueryHandler(IDbConnectionFactory connectionFactory, 
            IHostEnvironment environment, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<string> Handle(GetReportPengajuanAkseptasiQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            var datas = (await _connectionFactory.QueryProc<ReportPengajuanAkseptasiDto>("sp_ResumeAks", 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()},{request.kd_scob.Trim()}," +
                                $"{request.kd_thn},{request.no_aks.Trim()}"
                })).ToList();
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "PengajuanAkseptasi.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (datas.Count == 0)
                throw new NullReferenceException("Data tidak ditemukan");

            Template templateProfileResult = Template.Parse( templateReportHtml );

            var data = datas.FirstOrDefault();
            
            
            var wwwroot = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot");
            var path = _configuration.GetSection("UserSignature").Value.TrimEnd('/').TrimStart('/');

            var signature1 = ConvertImageToBase64Html(Path.Combine(wwwroot, path, data.user_id_dibuat ?? string.Empty, data.ttd_dibuat?.Replace("\\", "/") ?? string.Empty));
            var signature2 = ConvertImageToBase64Html(Path.Combine(wwwroot, path, data.user_id_diperiksa ?? string.Empty, data.ttd_diperiksa?.Replace("\\", "/") ?? string.Empty));
            var signature3 = ConvertImageToBase64Html(Path.Combine(wwwroot, path, data.user_id_disetujui ?? string.Empty, data.ttd_disetujui?.Replace("\\", "/") ?? string.Empty));

            var tgl_dibuat = data.tgl_dibuat == null ? string.Empty : data.tgl_dibuat.Value.ToString("dd MMM yyyy HH:mm:ss");
            var tgl_diperiksa = data.tgl_diperiksa == null ? string.Empty : data.tgl_diperiksa.Value.ToString("dd MMM yyyy HH:mm:ss");
            var tgl_disetujui = data.tgl_disetujui == null ? string.Empty : data.tgl_disetujui.Value.ToString("dd MMM yyyy HH:mm:ss");
            
            var resultTemplate = templateProfileResult.Render( new
            {
                data.nm_cb, data.nm_cob, data.nm_scob, data.nm_mkt, data.nm_sb_bis,
                data.nm_ttg, tgl_pengajuan = data.tgl_pengajuan.Value.ToString("dd/MM/yyyy"),
                tgl_mul_ptg = data.tgl_mul_ptg.Value.ToString("dd/MM/yyyy"),
                tgl_akh_ptg = data.tgl_akh_ptg.Value.ToString("dd/MM/yyyy"), data.symbol,
                nilai_pertanggungan = ReportHelper.ConvertToReportFormat(data.nilai_ttl_ptg),
                pst_dis = ReportHelper.ConvertToReportFormat(data.pst_dis, true),
                pst_kms = ReportHelper.ConvertToReportFormat(data.pst_kms, true),
                data.st_pas, pst_share = ReportHelper.ConvertToReportFormat(data.pst_share, true),
                data.no_pol_pas, data.nm_pas1, pst_pas1 = ReportHelper.ConvertToReportFormat(data.pst_pas1, true),
                data.nm_pas2, pst_pas2 = ReportHelper.ConvertToReportFormat(data.pst_pas2, true),
                data.nm_pas3, pst_pas3 = ReportHelper.ConvertToReportFormat(data.pst_pas3, true),
                data.nm_pas4, pst_pas4 = ReportHelper.ConvertToReportFormat(data.pst_pas4, true),
                data.nm_pas5, pst_pas5 = ReportHelper.ConvertToReportFormat(data.pst_pas5, true),
                data.ket_rsk, signature1, signature2, signature3, data.jabatan_dibuat, data.jabatan_diperiksa,
                data.jabatan_disetujui, data.nm_dibuat, data.nm_diperiksa, data.nm_disetujui,
                tgl_dibuat, tgl_diperiksa, tgl_disetujui, data.nomor_pengajuan
            } );

            return resultTemplate;
        }
        
        public static string ConvertImageToBase64Html(string imagePath)
        {
            try
            {

                if (!File.Exists(imagePath))
                    return string.Empty;
                
                // Read the image file
                byte[] imageBytes = File.ReadAllBytes(imagePath);
            
                // Convert to base64
                string base64String = Convert.ToBase64String(imageBytes);
            
                // Get the file extension to determine MIME type
                string extension = Path.GetExtension(imagePath).ToLower();
                string mimeType = extension switch
                {
                    ".jpg" => "image/jpeg",
                    ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    ".bmp" => "image/bmp",
                    _ => "application/octet-stream"
                };
            
                // Create HTML img tag with base64 source
                return $"<img src=\"data:{mimeType};base64,{base64String}\" alt=\"Signature\" style=\"max-height: 80px; max-width: 100%;\">";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting image: {ex.Message}");
                return string.Empty;
            }
        }
    }
}