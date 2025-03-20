using System;
using System.IO;
using System.Linq;
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

            var reportTempalteName = Constant.ReportMapping[templateName];
            
            if (!Constant.ReportStoreProcedureMapping.Keys.Contains(reportTempalteName))
                throw new Exception("Store Procedure Not Found");
            
            var storeProcedureName = Constant.ReportStoreProcedureMapping[reportTempalteName];
            
            var cetakNotaDanKwitansiPolis = (await _connectionFactory.QueryProc<CetakNotaDanKwitansiPolisDto>(storeProcedureName, 
                new
                {
                    input_str = $"{request.kd_cb.Trim()}{request.kd_cob.Trim()}{request.kd_scob.Trim()}" +
                                $"{request.kd_thn}{request.no_pol.Trim()}{request.no_updt}{request.nm_ttg?.Trim()}"
                })).FirstOrDefault();
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", reportTempalteName );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (cetakNotaDanKwitansiPolis == null)
                throw new NullReferenceException("Report tidak ditemukan");

            Template templateProfileResult = Template.Parse( templateReportHtml );
            
            string resultTemplate = templateProfileResult.Render( new
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

            return resultTemplate;
        }
    }
}