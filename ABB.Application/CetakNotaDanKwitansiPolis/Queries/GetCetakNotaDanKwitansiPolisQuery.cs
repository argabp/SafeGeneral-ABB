using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CetakSchedulePolis.Queries;
using ABB.Application.Common;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;
using Scriban;

namespace ABB.Application.CetakNotaDanKwitansiPolis.Queries
{
    public class GetCetakNotaDanKwitansiPolisQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public int kd_thn { get; set; }
        public string no_pol { get; set; }
        public string? nm_ttg { get; set; }
        public int no_updt { get; set; }
        public string jenisLaporan { get; set; }
        public string mataUang { get; set; }
    }

    public class GetCetakNotaDanKwitansiPolisQueryHandler : IRequestHandler<GetCetakNotaDanKwitansiPolisQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;

        private List<string> ReportHaveDetails = new List<string>()
        {
        };

        public GetCetakNotaDanKwitansiPolisQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
        }

        public async Task<string> Handle(GetCetakNotaDanKwitansiPolisQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var templateName = (await _connectionFactory.QueryProc<string>("spi_uw02r_01", 
                new
                {
                    kd_lap = request.jenisLaporan
                })).FirstOrDefault();

            if (templateName == null || !Constant.ReportMapping.Keys.Contains(templateName))
                throw new Exception("Template Not Found");

            var reportTemplateName = Constant.ReportMapping[templateName];
            
            if (!Constant.ReportStoreProcedureMapping.Keys.Contains(reportTemplateName))
                throw new Exception("Store Procedure Not Found");
            
            var storeProcedureName = Constant.ReportStoreProcedureMapping[reportTemplateName];
            
            var cetakNotaDanKwitansiPolisData = (await _connectionFactory.QueryProc<CetakNotaDanKwitansiPolisDto>(storeProcedureName, 
                new
                {
                    input_str = $"{request.kd_cb.Trim()}{request.kd_cob.Trim()}{request.kd_scob.Trim()}" +
                                $"{request.kd_thn}{request.no_pol.Trim()}{request.no_updt}{request.nm_ttg?.Trim()}"
                })).ToList();
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", reportTemplateName );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (cetakNotaDanKwitansiPolisData.Count == 0)
                throw new NullReferenceException("Report tidak ditemukan");

            Template templateProfileResult = Template.Parse( templateReportHtml );
            string resultTemplate;
            var cetakNotaDanKwitansiPolis = cetakNotaDanKwitansiPolisData.FirstOrDefault();
            if (ReportHaveDetails.Contains(reportTemplateName))
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var data in cetakNotaDanKwitansiPolisData)
                {
                    stringBuilder.Append(GenerateDetailReport(reportTemplateName, data));
                }
                
                resultTemplate = templateProfileResult.Render( new
                {
                    jns_nota = cetakNotaDanKwitansiPolis.jns_tr + " . " + 
                               cetakNotaDanKwitansiPolis.jns_nt_msk + " . " + 
                               cetakNotaDanKwitansiPolis.jns_nt_kel,
                    cetakNotaDanKwitansiPolis.no_nota, cetakNotaDanKwitansiPolis.tgl_nt_ind,
                    cetakNotaDanKwitansiPolis.no_reg, cetakNotaDanKwitansiPolis.no_ref,
                    cetakNotaDanKwitansiPolis.uraian_01, kd_mtu_symbol_1 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    cetakNotaDanKwitansiPolis.nilai_01, cetakNotaDanKwitansiPolis.nm_ttg,
                    cetakNotaDanKwitansiPolis.uraian_02, kd_mtu_symbol_5 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    cetakNotaDanKwitansiPolis.nilai_02, cetakNotaDanKwitansiPolis.no_pol_ttg,
                    cetakNotaDanKwitansiPolis.uraian_03, kd_mtu_symbol_7 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    cetakNotaDanKwitansiPolis.nilai_03, cetakNotaDanKwitansiPolis.periode_polis,
                    cetakNotaDanKwitansiPolis.uraian_04, kd_mtu_symbol_8 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    cetakNotaDanKwitansiPolis.nilai_04, cetakNotaDanKwitansiPolis.nm_scob,
                    cetakNotaDanKwitansiPolis.nilai_ttl_ptg, kd_mtu_symbol_6 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    cetakNotaDanKwitansiPolis.nilai_nt, cetakNotaDanKwitansiPolis.ket_nilai_nt,
                    cetakNotaDanKwitansiPolis.nm_cb, cetakNotaDanKwitansiPolis.nm_rek,
                    cetakNotaDanKwitansiPolis.nm_akun, cetakNotaDanKwitansiPolis.nm_ttj_kms,
                    cetakNotaDanKwitansiPolis.almt_ttj_kms, cetakNotaDanKwitansiPolis.kt_ttj_kms,
                    cetakNotaDanKwitansiPolis.no_npwp, cetakNotaDanKwitansiPolis,
                    kd_mtu_symbol_3 = cetakNotaDanKwitansiPolis.kd_mtu_symbol, kd_mtu_symbol_2 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    kd_mtu_symbol_4 = cetakNotaDanKwitansiPolis.kd_mtu_symbol, cetakNotaDanKwitansiPolis.ket_nt_kms,
                    total_nilai = cetakNotaDanKwitansiPolis.nilai_01 + cetakNotaDanKwitansiPolis.nilai_03 + cetakNotaDanKwitansiPolis.nilai_04,
                    cetakNotaDanKwitansiPolis.nilai_net_kms, kd_mtu_symbol_11 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    cetakNotaDanKwitansiPolis.pst_ppn, cetakNotaDanKwitansiPolis.nilai_ppn,
                    pst_pph_title = cetakNotaDanKwitansiPolis.pst_pph == 2 ? "PPH 23" : "PPH 21",
                    cetakNotaDanKwitansiPolis.pst_pph, cetakNotaDanKwitansiPolis.nilai_pph,
                    cetakNotaDanKwitansiPolis.pst_lain, kd_mtu_symbol_9 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    cetakNotaDanKwitansiPolis.kt_cb, cetakNotaDanKwitansiPolis.almt_ttg, cetakNotaDanKwitansiPolis.ket_kwi,
                    cetakNotaDanKwitansiPolis.period_polis, cetakNotaDanKwitansiPolis.kd_mtu_symbol
                } );
            }
            else
            {
                resultTemplate = templateProfileResult.Render( new
                {
                    jns_nota = cetakNotaDanKwitansiPolis.jns_tr + " . " + 
                               cetakNotaDanKwitansiPolis.jns_nt_msk + " . " + 
                               cetakNotaDanKwitansiPolis.jns_nt_kel,
                    cetakNotaDanKwitansiPolis.no_nota, cetakNotaDanKwitansiPolis.tgl_nt_ind,
                    cetakNotaDanKwitansiPolis.no_reg, cetakNotaDanKwitansiPolis.no_ref,
                    cetakNotaDanKwitansiPolis.uraian_01, kd_mtu_symbol_1 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    cetakNotaDanKwitansiPolis.nilai_01, cetakNotaDanKwitansiPolis.nm_ttg,
                    cetakNotaDanKwitansiPolis.uraian_02, kd_mtu_symbol_5 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    cetakNotaDanKwitansiPolis.nilai_02, cetakNotaDanKwitansiPolis.no_pol_ttg,
                    cetakNotaDanKwitansiPolis.uraian_03, kd_mtu_symbol_7 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    cetakNotaDanKwitansiPolis.nilai_03, cetakNotaDanKwitansiPolis.periode_polis,
                    cetakNotaDanKwitansiPolis.uraian_04, kd_mtu_symbol_8 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    cetakNotaDanKwitansiPolis.nilai_04, cetakNotaDanKwitansiPolis.nm_scob,
                    cetakNotaDanKwitansiPolis.nilai_ttl_ptg, kd_mtu_symbol_6 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    cetakNotaDanKwitansiPolis.nilai_nt, cetakNotaDanKwitansiPolis.ket_nilai_nt,
                    cetakNotaDanKwitansiPolis.nm_cb, cetakNotaDanKwitansiPolis.nm_rek,
                    cetakNotaDanKwitansiPolis.nm_akun, cetakNotaDanKwitansiPolis.nm_ttj_kms,
                    cetakNotaDanKwitansiPolis.almt_ttj_kms, cetakNotaDanKwitansiPolis.kt_ttj_kms,
                    cetakNotaDanKwitansiPolis.no_npwp, cetakNotaDanKwitansiPolis,
                    kd_mtu_symbol_3 = cetakNotaDanKwitansiPolis.kd_mtu_symbol, kd_mtu_symbol_2 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    kd_mtu_symbol_4 = cetakNotaDanKwitansiPolis.kd_mtu_symbol, cetakNotaDanKwitansiPolis.ket_nt_kms,
                    total_nilai = cetakNotaDanKwitansiPolis.nilai_01 + cetakNotaDanKwitansiPolis.nilai_03 + cetakNotaDanKwitansiPolis.nilai_04,
                    cetakNotaDanKwitansiPolis.nilai_net_kms, kd_mtu_symbol_11 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    cetakNotaDanKwitansiPolis.pst_ppn, cetakNotaDanKwitansiPolis.nilai_ppn,
                    pst_pph_title = cetakNotaDanKwitansiPolis.pst_pph == 2 ? "PPH 23" : "PPH 21",
                    cetakNotaDanKwitansiPolis.pst_pph, cetakNotaDanKwitansiPolis.nilai_pph,
                    cetakNotaDanKwitansiPolis.pst_lain, kd_mtu_symbol_9 = cetakNotaDanKwitansiPolis.kd_mtu_symbol,
                    cetakNotaDanKwitansiPolis.kt_cb, cetakNotaDanKwitansiPolis.almt_ttg, cetakNotaDanKwitansiPolis.ket_kwi,
                    cetakNotaDanKwitansiPolis.period_polis, cetakNotaDanKwitansiPolis.kd_mtu_symbol
                } );
            }

            return resultTemplate;
        }
        
        private string GenerateDetailReport(string reportType, CetakNotaDanKwitansiPolisDto data)
        {
            switch (reportType)
            {
                // case "LampiranPolisFireDaftarIsi.html":
                //     return @$"<tr style='height: 50px;'>
                //         <td style='font-weight: bold;' colspan='8'>{data.nm_grp_oby}</td>
                //         </tr>
                //         <tr>
                //             <td style='width: 3%;  text-align: right; vertical-align: top;' rowspan='2'>{data.no_oby}</td>
                //             <td style='width: 20%; text-align: left; vertical-align: top;' rowspan='2'>{data.desk_oby}</td>
                //             <td style='width: 20%; text-align: left; vertical-align: top;' rowspan='2'>{data.almt_rsk}</td>
                //             <td style='width: 10%; text-align: left; vertical-align: top;' rowspan='2'>{data.ket_okup}</td>
                //             <td style='width: 5%;  text-align: center; vertical-align: top;'>{data.kd_kls_konstr}</td>
                //             <td style='width: 10%; text-align: left; vertical-align: top;' rowspan='2'>if( {data.kd_penerangan} ='1','Listrik', 'Lain-lain')</td>
                //             <td style='width: 10%; text-align: left; vertical-align: top;' rowspan='2'>{data.symbol}</td>
                //             <td style='width: 10%; text-align: center; vertical-align: top;' rowspan='2'>{data.nilai_ttl_ptg}</td>
                //         </tr>
                //         <tr>
                //             <td style='vertical-align: top; text-align: center;'>{data.ket_rsk}</td>
                //         </tr>
                //         <tr>
                //             <td></td>
                //             <td></td>
                //             <td></td>
                //             <td></td>
                //             <td></td>
                //             <td style='font-weight: bold; text-align: center;' colspan='2'>{data.nm_grp_oby} : </td>
                //             <td style='font-weight: bold;'>sum( {data.nilai_ttl_ptg} for 1  ) </td>
                //         </tr>";
                default:
                    return string.Empty;
            }
        }
    }
}