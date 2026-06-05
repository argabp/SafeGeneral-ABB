using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CetakNotaPremiFakultatifKeluars.Queries;
using ABB.Application.CetakNotaPremiFakultatifMasuks.Queries;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Models;
using MediatR;
using Microsoft.Extensions.Hosting;
using Scriban;

namespace ABB.Application.CetakNotaPremiFakultatifKeluars.Commands
{
    public class CetakNotaPremiFakultatifKeluarCommand : IRequest<string>
    {
        public string kd_cb { get; set; }

        public string jns_tr { get; set; }

        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }

        public string flag_posting { get; set; }

        public string jns_lap { get; set; }
    }

    public class CetakNotaPremiFakultatifKeluarCommandHandler : IRequestHandler<CetakNotaPremiFakultatifKeluarCommand, string>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly IHostEnvironment _environment;
        private readonly ReportConfig _reportConfig;
        private readonly ReportTTDConfig _reportTtdConfig;

        public CetakNotaPremiFakultatifKeluarCommandHandler(IDbConnectionPst connectionPst, IHostEnvironment environment,
            ReportConfig reportConfig, ReportTTDConfig reportTtdConfig)
        {
            _connectionPst = connectionPst;
            _environment = environment;
            _reportConfig = reportConfig;
            _reportTtdConfig = reportTtdConfig;
        }

        public async Task<string> Handle(CetakNotaPremiFakultatifKeluarCommand request, CancellationToken cancellationToken)
        {
            var spName = string.Empty;
            switch (request.jns_lap)
            {
                case "1":
                    spName = "spr_ri02r_02";
                    break;
                case "2":
                    spName = "spr_ri02r_02_as";
                    break;
            }
            
            var datas = (await _connectionPst.QueryProc<CetakNotaPremiFakultatifKeluarModel>(spName, 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.jns_tr.Trim()},{request.jns_nt_msk.Trim()}," +
                                $"{request.kd_thn},{request.kd_bln.Trim()},{request.no_nt_msk.Trim()}," +
                                $"{request.jns_nt_kel},{request.no_nt_kel.Trim()},{request.flag_posting.Trim()},"
                })).ToList();
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "NotaPremiFakultatifKeluar.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (datas.Count == 0)
                throw new NullReferenceException("Data tidak ditemukan");

            Template templateProfileResult = Template.Parse( templateReportHtml );

            var data = datas.First();

            var jns_nota = string.Empty;

            switch (request.jns_lap)
            {
                case "1":
                    jns_nota = $"Jenis Nota : {data.jns_tr} . {data.jns_nt_msk} . {data.jns_nt_kel}";
                    break;
            }
            
            var reportConfig = _reportConfig.GetReportData(request.kd_cb);

            var nilai_01 = ReportHelper.ConvertToReportFormat(data.nilai_01);
            var nilai_02 = ReportHelper.ConvertToReportFormat(data.nilai_02);
            var nilai_03 = ReportHelper.ConvertToReportFormat(data.nilai_03);
            var nilai_04 = ReportHelper.ConvertToReportFormat(data.nilai_04);
            var nilai_05 = ReportHelper.ConvertToReportFormat(data.nilai_05);
            var nilai_nt = ReportHelper.ConvertToReportFormat(data.nilai_nt);
            var nilai_ttl_ptg = ReportHelper.ConvertToReportFormat(data.nilai_ttl_ptg);

            var sectionTemplate = @"
                            <tr>
                                <td style='width: 15%;'></td>
                                <td style='width: 20%;'>{uraian}</td>
                                <td style='width: 5%;'>:</td>
                                <td style='width: 15%;'>{symbol}</td>
                                <td style='text-align: right; width: 35%;'>{nilai}</td>
                                <td style='width: 5%;'></td>
                            <tr>";

            var totalSectionTemplate = @"
                            <tr>
                                <td></td>
                                <td>JUMLAH</td>
                                <td>:</td>
                                <td style='border-bottom: 3px double black; border-top: 1px solid black'>{symbol}</td>
                                <td style='text-align: right; border-bottom: 3px double black; border-top: 1px solid black'>{nilai}</td>
                                <td></td>
                            </tr>";

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

            var rincian_5 = ReportHelper.BuildSection(data.nilai_05, sectionTemplate, 
                new Dictionary<string, object>()
                {
                    { "uraian", data.uraian_05 },
                    { "symbol", data.kd_mtu_symbol },
                    { "nilai", nilai_05 }
                });

            var total = ReportHelper.BuildSection(data.nilai_nt, totalSectionTemplate, 
                new Dictionary<string, object>()
                {
                    { "symbol", data.kd_mtu_symbol },
                    { "nilai", nilai_nt }
                });

            var header = data.st_nota == "D" ? "NOTA DEBET" : "NOTA KREDIT";
            
            var resultTemplate = templateProfileResult.Render( new
            {
                data.no_nota, data.kd_cob, data.kd_scob, data.nm_scob, data.no_pol_ttg, nilai_01,
                jns_nota, rincian_2, rincian_3, rincian_4, total, data.nm_ttj, data.nm_ttg, rincian_5,
                data.ket_nt, tgl_mul = data.tgl_mul.ToString("dd MMM yyyy"), data.kd_mtu_symbol,
                tgl_akh = data.tgl_akh.ToString("dd MMM yyyy"), nilai_ttl_ptg, data.uraian_09,
                data.kt_ttj, data.tgl_nt_ind, title = reportConfig.Title.Title1, data.uraian_01, header
            } );

            return resultTemplate;
        }
    }
}