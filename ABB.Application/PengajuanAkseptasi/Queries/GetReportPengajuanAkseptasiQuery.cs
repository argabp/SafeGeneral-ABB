using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Scriban;

namespace ABB.Application.PengajuanAkseptasi.Queries
{
    public class GetReportPengajuanAkseptasiQuery : IRequest<(string, string)>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_aks { get; set; }
    }

    public class GetReportPengajuanAkseptasiQueryHandler : IRequestHandler<GetReportPengajuanAkseptasiQuery, (string, string)>
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

        public async Task<(string, string)> Handle(GetReportPengajuanAkseptasiQuery request, CancellationToken cancellationToken)
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
            
            var keterangan_resiko = GenerateKeteranganDokumen(data);
            
            var resultTemplate = templateProfileResult.Render( new
            {
                data.nm_cb, data.nm_cob, data.nm_scob, data.nm_mkt, data.nm_sb_bis,
                data.nm_ttg, tgl_pengajuan = data.tgl_pengajuan.Value.ToString("dd/MM/yyyy"),
                tgl_mul_ptg = data.tgl_mul_ptg.Value.ToString("dd/MM/yyyy"),
                tgl_akh_ptg = data.tgl_akh_ptg.Value.ToString("dd/MM/yyyy"), data.symbol,
                nilai_pertanggungan = ReportHelper.ConvertToReportFormat(data.nilai_ttl_ptg),
                pst_dis = ReportHelper.ConvertToReportFormat(data.pst_dis),
                pst_kms = ReportHelper.ConvertToReportFormat(data.pst_kms),
                data.st_pas, pst_share = ReportHelper.ConvertToReportFormat(data.pst_share),
                data.no_pol_pas, data.nm_pas1, pst_pas1 = ReportHelper.ConvertToReportFormat(data.pst_pas1),
                data.nm_pas2, pst_pas2 = ReportHelper.ConvertToReportFormat(data.pst_pas2),
                data.nm_pas3, pst_pas3 = ReportHelper.ConvertToReportFormat(data.pst_pas3),
                data.nm_pas4, pst_pas4 = ReportHelper.ConvertToReportFormat(data.pst_pas4),
                data.nm_pas5, pst_pas5 = ReportHelper.ConvertToReportFormat(data.pst_pas5),
                data.ket_rsk, signature1, signature2, signature3, data.jabatan_dibuat, data.jabatan_diperiksa,
                data.jabatan_disetujui, data.nm_dibuat, data.nm_diperiksa, data.nm_disetujui,
                tgl_dibuat, tgl_diperiksa, tgl_disetujui, data.nomor_pengajuan, keterangan_resiko,
                nilai_tsi_share = ReportHelper.ConvertToReportFormat(data.nilai_tsi_share), data.nm_tol,
                data.maks_panel, nilai_kapasitas = ReportHelper.ConvertToReportFormat(data.nilai_kapasitas),
                pst_tol = ReportHelper.ConvertToReportFormat(data.pst_tol),
                pst_mul1 = ReportHelper.ConvertToReportFormat(data.pst_mul1),
                pst_akh1 = ReportHelper.ConvertToReportFormat(data.pst_akh1),
                pst_koas1 = ReportHelper.ConvertToReportFormat(data.pst_koas1),
                nilai_kapasitas_tty1 = ReportHelper.ConvertToReportFormat(data.nilai_kapasitas_tty1),
                nilai_limit_tsi1 = ReportHelper.ConvertToReportFormat(data.nilai_limit_tsi1),
                nilai_limit_sharemax1 = ReportHelper.ConvertToReportFormat(data.nilai_limit_sharemax1),
                pst_mul2 = ReportHelper.ConvertToReportFormat(data.pst_mul2),
                pst_akh2 = ReportHelper.ConvertToReportFormat(data.pst_akh2),
                pst_koas2 = ReportHelper.ConvertToReportFormat(data.pst_koas2),
                nilai_kapasitas_tty2 = ReportHelper.ConvertToReportFormat(data.nilai_kapasitas_tty2),
                nilai_limit_tsi2 = ReportHelper.ConvertToReportFormat(data.nilai_limit_tsi2),
                nilai_limit_sharemax2 = ReportHelper.ConvertToReportFormat(data.nilai_limit_sharemax2),
                pst_mul3 = ReportHelper.ConvertToReportFormat(data.pst_mul3),
                pst_akh3 = ReportHelper.ConvertToReportFormat(data.pst_akh3),
                pst_koas3 = ReportHelper.ConvertToReportFormat(data.pst_koas3),
                nilai_kapasitas_tty3 = ReportHelper.ConvertToReportFormat(data.nilai_kapasitas_tty3),
                nilai_limit_tsi3 = ReportHelper.ConvertToReportFormat(data.nilai_limit_tsi3),
                nilai_limit_sharemax3 = ReportHelper.ConvertToReportFormat(data.nilai_limit_sharemax3),
                pst_mul4 = ReportHelper.ConvertToReportFormat(data.pst_mul4),
                pst_akh4 = ReportHelper.ConvertToReportFormat(data.pst_akh4),
                pst_koas4 = ReportHelper.ConvertToReportFormat(data.pst_koas4),
                nilai_kapasitas_tty4 = ReportHelper.ConvertToReportFormat(data.nilai_kapasitas_tty4),
                nilai_limit_tsi4 = ReportHelper.ConvertToReportFormat(data.nilai_limit_tsi4),
                nilai_limit_sharemax4 = ReportHelper.ConvertToReportFormat(data.nilai_limit_sharemax4),
                pst_limit_cab = ReportHelper.ConvertToReportFormat(data.pst_limit_cab)
            } );

            var secondDatas = (await _connectionFactory.QueryProc<ReportKeteranganPengajuanAkseptasiDto>("sp_KeteranganStatusAks", 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()},{request.kd_scob.Trim()}," +
                                $"{request.kd_thn},{request.no_aks.Trim()}"
                })).ToList();
            
            string secondReportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "KeteranganPengajuanAkseptasi.html" );
            
            string secondTemplateReportHtml = await File.ReadAllTextAsync( secondReportPath );

            var nomor_pengajuan = string.Empty;

            if (secondDatas.Any())
            {
                nomor_pengajuan = secondDatas[0].nomor_pengajuan;
            }
            
            Template secondTemplateProfileResult = Template.Parse( secondTemplateReportHtml );
            
            var detail = new StringBuilder();
            var counter = 0;
            
            foreach (var secondData in secondDatas)
            {
                counter++;
                
                var second_tgl_dibuat = secondData.tgl_status == null ? string.Empty : secondData.tgl_status.Value.ToString("dd MMM yyyy HH:mm:ss");

                detail.Append($@"<tr style='border: 1px dashed black'>
                    <td style='border: 1px dashed black'>{counter}</td>
                    <td style='border: 1px dashed black'>{secondData.nm_user}</td>
                    <td style='border: 1px dashed black'>{second_tgl_dibuat}</td>
                    <td style='border: 1px dashed black'>{secondData.nm_status}</td>
                    <td style='border: 1px dashed black'>{secondData.ket_status}</td>
                </tr>");
            }
            
            var secondResultTemplate = secondTemplateProfileResult.Render( new
            {
                nomor_pengajuan, detail = detail.ToString() 
            } );

            return (resultTemplate, secondResultTemplate);
        }

        private string GenerateKeteranganDokumen(ReportPengajuanAkseptasiDto reportPengajuanAkseptasiDto)
        {
            StringBuilder sb = new StringBuilder();

            var nm_dokumen1 = string.IsNullOrWhiteSpace(reportPengajuanAkseptasiDto.nm_dokumen1) ? string.Empty : reportPengajuanAkseptasiDto.nm_dokumen1;
            var st_dokumen1 = reportPengajuanAkseptasiDto.st_dokumen1 != null && reportPengajuanAkseptasiDto.st_dokumen1.Value ? "checked" : string.Empty;
            var nm_dokumen2 = string.IsNullOrWhiteSpace(reportPengajuanAkseptasiDto.nm_dokumen2) ? string.Empty : reportPengajuanAkseptasiDto.nm_dokumen2;
            var st_dokumen2 = reportPengajuanAkseptasiDto.st_dokumen2 != null && reportPengajuanAkseptasiDto.st_dokumen2.Value ? "checked" : string.Empty;
            var nm_dokumen3 = string.IsNullOrWhiteSpace(reportPengajuanAkseptasiDto.nm_dokumen3) ? string.Empty : reportPengajuanAkseptasiDto.nm_dokumen3;
            var st_dokumen3 = reportPengajuanAkseptasiDto.st_dokumen3 != null && reportPengajuanAkseptasiDto.st_dokumen3.Value ? "checked" : string.Empty;
            var nm_dokumen4 = string.IsNullOrWhiteSpace(reportPengajuanAkseptasiDto.nm_dokumen4) ? string.Empty : reportPengajuanAkseptasiDto.nm_dokumen4;
            var st_dokumen4 = reportPengajuanAkseptasiDto.st_dokumen4 != null && reportPengajuanAkseptasiDto.st_dokumen4.Value ? "checked" : string.Empty;
            var nm_dokumen5 = string.IsNullOrWhiteSpace(reportPengajuanAkseptasiDto.nm_dokumen5) ? string.Empty : reportPengajuanAkseptasiDto.nm_dokumen5;
            var st_dokumen5 = reportPengajuanAkseptasiDto.st_dokumen5 != null && reportPengajuanAkseptasiDto.st_dokumen5.Value ? "checked" : string.Empty;
            var nm_dokumen6 = string.IsNullOrWhiteSpace(reportPengajuanAkseptasiDto.nm_dokumen6) ? string.Empty : reportPengajuanAkseptasiDto.nm_dokumen6;
            var st_dokumen6 = reportPengajuanAkseptasiDto.st_dokumen6 != null && reportPengajuanAkseptasiDto.st_dokumen6.Value ? "checked" : string.Empty;
            var nm_dokumen7 = string.IsNullOrWhiteSpace(reportPengajuanAkseptasiDto.nm_dokumen7) ? string.Empty : reportPengajuanAkseptasiDto.nm_dokumen7;
            var st_dokumen7 = reportPengajuanAkseptasiDto.st_dokumen7 != null && reportPengajuanAkseptasiDto.st_dokumen7.Value ? "checked" : string.Empty;
            var nm_dokumen8 = string.IsNullOrWhiteSpace(reportPengajuanAkseptasiDto.nm_dokumen8) ? string.Empty : reportPengajuanAkseptasiDto.nm_dokumen8;
            var st_dokumen8 = reportPengajuanAkseptasiDto.st_dokumen8 != null && reportPengajuanAkseptasiDto.st_dokumen8.Value ? "checked" : string.Empty;
            var nm_dokumen9 = string.IsNullOrWhiteSpace(reportPengajuanAkseptasiDto.nm_dokumen9) ? string.Empty : reportPengajuanAkseptasiDto.nm_dokumen9;
            var st_dokumen9 = reportPengajuanAkseptasiDto.st_dokumen9 != null && reportPengajuanAkseptasiDto.st_dokumen9.Value ? "checked" : string.Empty;
            var nm_dokumen10 = string.IsNullOrWhiteSpace(reportPengajuanAkseptasiDto.nm_dokumen10) ? string.Empty : reportPengajuanAkseptasiDto.nm_dokumen10;
            var st_dokumen10 = reportPengajuanAkseptasiDto.st_dokumen10 != null && reportPengajuanAkseptasiDto.st_dokumen10.Value ? "checked" : string.Empty;
            
            sb.Append(@"
<table cellspacing='0' cellpadding='5' width='100%'  style='margin-bottom: 0px'>");
            
            sb.Append($"<tr><td style='width: 25%'>{nm_dokumen1}</td>");
            sb.Append(string.IsNullOrWhiteSpace(nm_dokumen1)
                ? "<td style='width: 25%'></td>"
                : $"<td style='width: 25%'><input type='checkbox' {st_dokumen1} /></td>");
            sb.Append($"<td style='width: 25%'>{nm_dokumen6}</td>");
            sb.Append(string.IsNullOrWhiteSpace(nm_dokumen6)
                ? "<td style='width: 25%'></td>"
                : $"<td style='width: 25%'><input type='checkbox' {st_dokumen6} /></td></tr>");
            
            sb.Append($"<tr><td style='width: 25%'>{nm_dokumen2}</td>");
            sb.Append(string.IsNullOrWhiteSpace(nm_dokumen2)
                ? "<td style='width: 25%'></td>"
                : $"<td style='width: 25%'><input type='checkbox' {st_dokumen2} /></td>");
            sb.Append($"<td style='width: 25%'>{nm_dokumen7}</td>");
            sb.Append(string.IsNullOrWhiteSpace(nm_dokumen7)
                ? "<td style='width: 25%'></td>"
                : $"<td style='width: 25%'><input type='checkbox' {st_dokumen7} /></td></tr>");
            
            sb.Append($"<tr><td style='width: 25%'>{nm_dokumen3}</td>");
            sb.Append(string.IsNullOrWhiteSpace(nm_dokumen3)
                ? "<td></td>"
                : $"<td style='width: 25%'><input type='checkbox' {st_dokumen3} /></td>");
            sb.Append($"<td style='width: 25%'>{nm_dokumen8}</td>");
            sb.Append(string.IsNullOrWhiteSpace(nm_dokumen8)
                ? "<td style='width: 25%'></td>"
                : $"<td style='width: 25%'><input type='checkbox' {st_dokumen8} /></td></tr>");
            
            sb.Append($"<tr><td style='width: 25%'>{nm_dokumen4}</td>");
            sb.Append(string.IsNullOrWhiteSpace(nm_dokumen4)
                ? "<td style='width: 25%'></td>"
                : $"<td style='width: 25%'><input type='checkbox' {st_dokumen4} /></td>");
            sb.Append($"<td style='width: 25%'>{nm_dokumen9}</td>");
            sb.Append(string.IsNullOrWhiteSpace(nm_dokumen9)
                ? "<td style='width: 25%'></td>"
                : $"<td style='width: 25%'><input type='checkbox' {st_dokumen9} /></td></tr>");
            
            sb.Append($"<tr><td style='width: 25%'>{nm_dokumen5}</td>");
            sb.Append(string.IsNullOrWhiteSpace(nm_dokumen5)
                ? "<td style='width: 25%'></td>"
                : $"<td style='width: 25%'><input type='checkbox' {st_dokumen5} /></td>");
            sb.Append($"<td style='width: 25%'>{nm_dokumen10}</td>");
            sb.Append(string.IsNullOrWhiteSpace(nm_dokumen10)
                ? "<td style='width: 25%'></td>"
                : $"<td style='width: 25%'><input type='checkbox' {st_dokumen10} /></td></tr>");

            sb.Append("</table>");
            
            return sb.ToString();
        }
        
        private string ConvertImageToBase64Html(string imagePath)
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