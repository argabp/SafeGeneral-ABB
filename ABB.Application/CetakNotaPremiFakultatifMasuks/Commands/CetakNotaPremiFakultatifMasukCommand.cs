using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CetakNotaPremiFakultatifMasuks.Queries;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Models;
using MediatR;
using Microsoft.Extensions.Hosting;
using Scriban;

namespace ABB.Application.CetakNotaPremiFakultatifMasuks.Commands
{
    public class CetakNotaPremiFakultatifMasukCommand : IRequest<string>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public short no_updt { get; set; }

        public string jns_lap { get; set; }

        public string kd_mtu { get; set; }
    }

    public class CetakNotaPremiFakultatifMasukCommandHandler : IRequestHandler<CetakNotaPremiFakultatifMasukCommand, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;
        private readonly ReportConfig _reportConfig;
        private readonly ReportTTDConfig _reportTtdConfig;

        public CetakNotaPremiFakultatifMasukCommandHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment,
            ReportConfig reportConfig, ReportTTDConfig reportTtdConfig)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
            _reportConfig = reportConfig;
            _reportTtdConfig = reportTtdConfig;
        }

        public async Task<string> Handle(CetakNotaPremiFakultatifMasukCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection("abb_kp00");
            
            var datas = (await _connectionFactory.QueryProc<CetakNotaPremiFakultatifMasukModel>("spr_uw02r_01_n", 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()},{request.kd_scob.Trim()}," +
                                $"{request.kd_thn},{request.no_pol.Trim()},{request.no_updt}," +
                                $"{request.jns_lap},{request.kd_mtu.Trim()}"
                })).ToList();
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "NotaPremiFakultatifMasuk.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (datas.Count == 0)
                throw new NullReferenceException("Data tidak ditemukan");

            Template templateProfileResult = Template.Parse( templateReportHtml );

            var footer_template = string.Empty;
            var ttd_image = string.Empty;
            var nm_pejabat = string.Empty;
            var jabatan = string.Empty;

            var data = datas.FirstOrDefault();
            if (data.kd_cb.Length >= 4 && data.kd_cb.Substring(3, 1) != "0")
            {
                var ttdImageBase64 = string.Empty;
                var wwwroot = Path.Combine(_environment.ContentRootPath, "wwwroot", "img");
                var imageFile = Path.Combine(wwwroot, _reportTtdConfig.NamaTTDFile);
                if (File.Exists(imageFile))
                {
                    ttdImageBase64 = Convert.ToBase64String(File.ReadAllBytes(imageFile));
                }
                string extension = Path.GetExtension(imageFile).ToLower();
                string mimeType = extension switch
                {
                    ".png" => "image/png",
                    ".jpg" => "image/jpeg",
                    ".jpeg" => "image/jpeg",
                    ".gif" => "image/gif",
                    ".bmp" => "image/bmp",
                    _ => "application/octet-stream"
                };
                ttd_image = $"<img src='data:${mimeType};base64,${ttdImageBase64}' style='max-width: 200px;' />";
                nm_pejabat = $"<p style='margin: 0'><strong><u>{_reportTtdConfig.NamaPejabat}</u></strong></p>";
                jabatan = $"<p style='margin: 0'><strong>{_reportTtdConfig.Jabatan}<strong></p>";
                footer_template = $@"<div style='text-align: center;'>{ttd_image}{nm_pejabat}{jabatan}</div>";
            }
            
            var reportConfig = _reportConfig.GetReportData(request.kd_cb);

            var nilai_01 = ReportHelper.ConvertToReportFormat(data.nilai_01);
            var nilai_02 = ReportHelper.ConvertToReportFormat(data.nilai_02);
            var nilai_03 = ReportHelper.ConvertToReportFormat(data.nilai_03);
            var nilai_04 = ReportHelper.ConvertToReportFormat(data.nilai_04);
            var nilai_nt = ReportHelper.ConvertToReportFormat(data.nilai_nt);
            var nilai_ttl_ptg = ReportHelper.ConvertToReportFormat(data.nilai_ttl_ptg);
            var nilai_share = (data.nilai_ttl_ptg ?? 0 * data.pst_pjk ?? 0) / 100;

            var share = data.pst_pjk / 100;

            var sectionTemplate = @"
	                <td style='width: 5%'>{uraian}</td>
	                <td style='width: 1%'>:</td>
	                <td style='width: 5%'>{symbol}</td>
	                <td style='text-align: right; width: 5%'>{nilai}</td>";

            var rincian_1 = ReportHelper.BuildSection(data.nilai_01, sectionTemplate, 
                new Dictionary<string, object>()
            {
                { "uraian", data.uraian_01 },
                { "symbol", data.kd_mtu_symbol },
                { "nilai", nilai_01 }
            });

            var rincian_2 = ReportHelper.BuildSection(data.nilai_02, sectionTemplate, 
                new Dictionary<string, object>()
                {
                    { "uraian", data.uraian_02 },
                    { "symbol", data.kd_mtu_symbol },
                    { "nilai", nilai_02 }
                });

            var rincian_3 = ReportHelper.BuildSection(data.nilai_03, sectionTemplate, 
                new Dictionary<string, object>()
                {
                    { "uraian", data.uraian_03 },
                    { "symbol", data.kd_mtu_symbol },
                    { "nilai", nilai_03 }
                });

            var rincian_4 = ReportHelper.BuildSection(data.nilai_04, sectionTemplate, 
                new Dictionary<string, object>()
                {
                    { "uraian", data.uraian_04 },
                    { "symbol", data.kd_mtu_symbol },
                    { "nilai", nilai_04 }
                });
            
            var resultTemplate = templateProfileResult.Render( new
            {
                nilai_share, footer_template, data.nm_nota, data.almt_nota, share, data.nm_scob,
                jns_nota = data.jns_tr + " . " + data.jns_nt_msk + " . " + data.jns_nt_kel,
                data.no_nota, rincian_1, rincian_2, rincian_3, rincian_4, data.tgl_nt_ind, data.no_reg, 
                data.no_ref, data.period_polis, nilai_ttl_ptg, data.nm_cb, nilai_nt, data.kt_cb, 
                data.wpc, data.nm_ttg, data.ket_nilai_nt, data.nm_rek, data.nm_akun,data.kd_mtu_symbol,
                title3 = reportConfig.Title.Title3, title4 = reportConfig.Title.Title4, title6 = reportConfig.Title.Title6
            } );

            return resultTemplate;
        }
    }
}