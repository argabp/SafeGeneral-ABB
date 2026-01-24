using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CetakNotaKlaim.Queries;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Models;
using MediatR;
using Microsoft.Extensions.Hosting;
using Scriban;

namespace ABB.Application.CetakNotaKoasuransiKeluars.Queries
{
    public class GetCetakNotaKoasuransiKeluarQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string jns_tr { get; set; }
        public string jns_nt_msk { get; set; }
        public string kd_thn { get; set; }
        public string kd_bln { get; set; }
        public string no_nt_msk { get; set; }
        public string jns_nt_kel { get; set; }
        public string no_nt_kel { get; set; }
        public string flag_posting { get; set; }
    }

    public class GetCetakNotaKoasuransiKeluarQueryHandler : IRequestHandler<GetCetakNotaKoasuransiKeluarQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;
        private readonly ReportConfig _reportConfig;

        public GetCetakNotaKoasuransiKeluarQueryHandler(IDbConnectionFactory connectionFactory, 
	        IHostEnvironment environment, ReportConfig reportConfig)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
            _reportConfig = reportConfig;
        }

        public async Task<string> Handle(GetCetakNotaKoasuransiKeluarQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            var datas = (await _connectionFactory.QueryProc<CetakNotaKoasuransiKeluarModel>("spr_uw02r_02_n", 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.jns_tr.Trim()},{request.jns_nt_msk.Trim()}," +
                                $"{request.kd_thn},{request.kd_bln.Trim()},{request.no_nt_msk.Trim()}," +
                                $"{request.jns_nt_kel},{request.no_nt_kel.Trim()},{request.flag_posting.Trim()}"
                })).ToList();
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "CetakanNotaKoasuransiKeluar.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (datas.Count == 0)
                throw new NullReferenceException("Data tidak ditemukan");

            var reportConfig = _reportConfig.GetReportData(request.kd_cb);
            
            Template templateProfileResult = Template.Parse( templateReportHtml );

            var data = datas.FirstOrDefault();
            
            var nilai_nt = ReportHelper.ConvertToReportFormat(data.nilai_nt);

            var div_nilai_01 = data.nilai_01 == 0
                ? $@"
		            <td style='width: 5%'></td>
		            <td style='width: 1%'></td>
		            <td style='width: 5%'></td>
		            <td style='text-align: right; width: 5%'></td>"
                : $@"
		<td style='width: 5%'>{data.uraian_01}</td>
		<td style='width: 1%'>:</td>
		<td style='width: 1%'>{data.kd_mtu_symbol}</td>
		<td style='text-align: right; width: 5%'>{ReportHelper.ConvertToReportFormat(data.nilai_01)}</td>";

            var div_nilai_02 = data.nilai_02 == 0
                ? $@"
		            <td style='width: 5%'></td>
		            <td style='width: 1%'></td>
		            <td style='width: 5%'></td>
		            <td style='text-align: right; width: 5%'></td>"
                : $@"
		<td style='width: 5%'>{data.uraian_02}</td>
		<td style='width: 1%'>:</td>
		<td style='width: 1%'>{data.kd_mtu_symbol}</td>
		<td style='text-align: right; width: 5%'>{ReportHelper.ConvertToReportFormat(data.nilai_02)}</td>";

            var div_nilai_03 = data.nilai_03 == 0
                ? $@"
		            <td style='width: 5%'></td>
		            <td style='width: 1%'></td>
		            <td style='width: 5%'></td>
		            <td style='text-align: right; width: 5%'></td>"
                : $@"
		<td style='width: 5%'>{data.uraian_03}</td>
		<td style='width: 1%'>:</td>
		<td style='width: 1%'>{data.kd_mtu_symbol}</td>
		<td style='text-align: right; width: 5%'>{ReportHelper.ConvertToReportFormat(data.nilai_03)}</td>";

            var div_nilai_04 = data.nilai_04 == 0
                ? $@"
		            <td style='width: 5%'></td>
		            <td style='width: 1%'></td>
		            <td style='width: 5%'></td>
		            <td style='text-align: right; width: 5%'></td>"
                : $@"
		<td style='width: 5%'>{data.uraian_04}</td>
		<td style='width: 1%'>:</td>
		<td style='width: 1%'>{data.kd_mtu_symbol}</td>
		<td style='text-align: right; width: 5%'>{ReportHelper.ConvertToReportFormat(data.nilai_04)}</td>";

            var div_nilai_05 = data.nilai_05 == 0
                ? $@"
		            <td style='width: 5%'></td>
		            <td style='width: 1%'></td>
		            <td style='width: 5%'></td>
		            <td style='text-align: right; width: 5%'></td>"
                : $@"
		<td style='width: 5%'>{data.uraian_05}</td>
		<td style='width: 1%'>:</td>
		<td style='width: 1%'>{data.kd_mtu_symbol}</td>
		<td style='text-align: right; width: 5%'>{ReportHelper.ConvertToReportFormat(data.nilai_05)}</td>";
            var header = data.st_nota == "D" ? "NOTA DEBET" : "NOTA KREDIT";
            var nilai_ttl_ptg = ReportHelper.ConvertToReportFormat(data.nilai_ttl_ptg);
            var nilai_prm_full = ReportHelper.ConvertToReportFormat(data.nilai_prm_full);
            
            var resultTemplate = templateProfileResult.Render( new
            {
                nilai_nt, div_nilai_01, div_nilai_02, div_nilai_03, div_nilai_04, div_nilai_05, 
                data.jns_tr, data.jns_nt_msk, data.jns_nt_kel, data.nm_cb, data.almt_ttj,
                data.almt_ttg, data.flag_postr, data.nm_ttg, data.nm_ttj, data.kt_ttj,
                data.no_berkas, data.no_pol_ttg, data.kt_ttg, data.kd_mtu_symbol, data.kd_tl,
                data.nm_kt_cb, data.tgl_nt_ind, data.no_nota, data.nm_scob, data.nm_scob_ing,
                data.kd_cob, data.kd_scob, nilai_share_bgu = ReportHelper.ConvertToReportFormat(data.nilai_share_bgu), data.ket_nt,
                tgl_mul_ptg = ReportHelper.ConvertDateTime(data.tgl_mul_ptg, "dd/MM/yyyy"),
                tgl_akh_ptg = ReportHelper.ConvertDateTime(data.tgl_akh_ptg, "dd/MM/yyyy"),
                data.uraian_01, data.uraian_02, data.uraian_03, data.uraian_04, data.uraian_05,
                data.kd_mtu_pol_symbol, data.ket_nilai_nt, data.period_polis, header, nilai_ttl_ptg, nilai_prm_full,
                title3 = reportConfig.Title.Title3, title4 = reportConfig.Title.Title4, title6 = reportConfig.Title.Title6
            } );

            return resultTemplate;
        }
    }
}