using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;
using Scriban;

namespace ABB.Application.LaporanProduksiAsuransiKeluar.Queries
{
    public class GetLaporanProduksiAsuransiKeluarQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public DateTime kd_bln_mul { get; set; }
        public DateTime kd_bln_akh { get; set; }
        public string? kd_thn { get; set; }
        public string kd_cb { get; set; }
        public string kd_grp_sb_bis { get; set; }
        public string kd_rk_sb_bis { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string? prm_th { get; set; }
        public string kd_grp_ttg { get; set; }
        public string kd_rk_ttg { get; set; }
        public string kd_grp_mkt { get; set; }
        public string kd_rk_mkt { get; set; }

        public decimal nilai_ttl_ptg { get; set; }
    }

    public class GetLaporanProduksiAsuransiKeluarQueryHandler : IRequestHandler<GetLaporanProduksiAsuransiKeluarQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;

        public GetLaporanProduksiAsuransiKeluarQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
        }

        public async Task<string> Handle(GetLaporanProduksiAsuransiKeluarQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            var kd_thn = request.kd_thn == null ? string.Empty : request.kd_thn.Trim();
            var laporanProduksiAsuransiKeluarDatas = (await _connectionFactory.QueryProc<LaporanProduksiAsuransiKeluarDto>("spr_uw06r_01", 
                new
                {
                    input_str = $"{request.kd_bln_mul.ToShortDateString()},{request.kd_bln_akh.ToShortDateString()},{kd_thn}," +
                                $"{request.kd_cb.Trim()},{request.kd_cob.Trim()},{request.kd_scob.Trim()},{request.nilai_ttl_ptg}" +
                                $"{request.kd_grp_sb_bis.Trim()},{request.kd_rk_sb_bis.Trim()},{request.kd_grp_ttg.Trim()}" +
                                $"{request.kd_rk_ttg.Trim()}"
                })).ToList();

            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "LaporanProduksiAsuransiKeluar.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (laporanProduksiAsuransiKeluarDatas.Count == 0)
            {
                throw new NullReferenceException("Data tidak ditemukan");
            }
            
            Template templateProfileResult = Template.Parse( templateReportHtml );

            var nama_cobs = laporanProduksiAsuransiKeluarDatas.Select(s => s.nm_cob).Distinct().ToList();

            string resultTemplate;
            
            StringBuilder stringBuilder = new StringBuilder();
            decimal total_semua_sum_ins = 0;
            decimal total_semua_pst_share = 0;
            decimal total_semua_nilai_prm = 0;
            decimal total_semua_nilai_kms = 0;
            decimal total_semua_nilai_hf = 0;
            decimal total_semua_nilai_net = 0;
            foreach (var nama_cob in nama_cobs)
            {
                var sequence = 0;
                var laporanProduksiAsuransiKeluar = laporanProduksiAsuransiKeluarDatas.FirstOrDefault(w => w.nm_cob == nama_cob);
                stringBuilder.Append(@$"<div style='page-break-before: always;'>
                                        <p style='font-size: 14px; margin: auto;'><strong>PT. ASURANSI BHAKTI BHAYANGKARA</strong></p>

                                        <div class='container'>
                                            <div class='section'>
                                                <p style='font-size: 14px; margin: auto; text-align: center;'>BUKU PRODUKSI KOASURANSI KELUAR</p>
                                                <p style='font-size: 14px; margin: auto; text-align: center;'>JENIS PERTANGGUNGAN : {laporanProduksiAsuransiKeluar.nm_cob}</p>
                                                <p style='font-size: 14px; margin: auto; text-align: center;'>PERIODE: {laporanProduksiAsuransiKeluar.periode}</p>
                                                <table class='table'>
                                                    <tr>
                                                        <td style='width: 3%; text-align: center; border: 1px solid'>NO</td>
                                                        <td style='width: 20%; text-align: center; border: 1px solid'>NOMOR/TANGGAL NOTA</td>
                                                        <td style='width: 20%; text-align: center; border: 1px solid'>MEMBER</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid'>NAMA TERTANGGUNG</td>
                                                        <td style='width: 5%;  text-align: center; border: 1px solid'>NOMOR POLIS<br>NO. REG</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid'>TOTAL SUM INS</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid'>SHARE MEMBER</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid'>PREMI KO. ASS.</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid'>BROKERAGE COMISSION</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid'>HANDLING FEE</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid'>AMOUNT DUE TO YOU</td>
                                                    </tr>");
                decimal total_sum_ins = 0;
                decimal total_pst_share = 0;
                decimal total_nilai_prm = 0;
                decimal total_nilai_kms = 0;
                decimal total_nilai_hf = 0;
                decimal total_nilai_net = 0;
                foreach (var data in laporanProduksiAsuransiKeluarDatas.Where(w => w.nm_cob == nama_cob))
                {
                    sequence++;
                    var nilai_ttl_ptg = ReportHelper.ConvertToReportFormat(data.nilai_ttl_ptg);
                    var pst_share = ReportHelper.ConvertToReportFormat(data.pst_share);
                    var nilai_prm = ReportHelper.ConvertToReportFormat(data.nilai_prm);
                    var nilai_kms = ReportHelper.ConvertToReportFormat(data.nilai_kms);
                    var nilai_hf = ReportHelper.ConvertToReportFormat(data.nilai_ttl_ptg);
                    var nilai_net = ReportHelper.ConvertToReportFormat(data.nilai_ttl_ptg);
                    stringBuilder.Append(@$"<tr>
                                                <td style='width: 3%;  text-align: left; vertical-align: top; border: 1px solid'>{sequence}</td>
                                                <td style='width: 20%; text-align: left; vertical-align: top; border: 1px solid'>{data.no_nota}<br>{data.tgl_nt}</td>
                                                <td style='width: 20%; text-align: left; vertical-align: top; border: 1px solid'>{data.nm_pas}</td>
                                                <td style='width: 20%; text-align: left; vertical-align: top; border: 1px solid'>{data.nm_ttg}</td>
                                                <td style='width: 10%; text-align: center; vertical-align: top; border: 1px solid'>{data.no_pol_ttg}<br>{data.no_reg}</td>
                                                <td style='width: 5%;  text-align: right; vertical-align: top; border: 1px solid'>{nilai_ttl_ptg}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{pst_share}</td>
                                                <td style='width: 10%; text-align: left; vertical-align: top; border: 1px solid'>{nilai_prm}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{nilai_kms}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{nilai_hf}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{nilai_net}</td>
                                            </tr>");
                    total_sum_ins += ReportHelper.ConvertToDecimalFormat(nilai_ttl_ptg);
                    total_pst_share += ReportHelper.ConvertToDecimalFormat(pst_share);
                    total_nilai_prm += ReportHelper.ConvertToDecimalFormat(nilai_prm);
                    total_nilai_kms += ReportHelper.ConvertToDecimalFormat(nilai_kms);
                    total_nilai_hf += ReportHelper.ConvertToDecimalFormat(nilai_hf);
                    total_nilai_net += ReportHelper.ConvertToDecimalFormat(nilai_net);
                }

                stringBuilder.Append(@$"<tr>
                                            <td colspan=1></td>
                                        </tr>
                                        <tr>
                                            <td colspan=5 style='border-bottom: 1px solid; border-top: 1px solid'>Sub Total Nilai</td>
                                            <td style='border-bottom: 1px solid; border-top: 1px solid; text-align: right'>{total_sum_ins:#,##0}</td>
                                            <td style='border-bottom: 1px solid; border-top: 1px solid; text-align: right'>{total_pst_share:#,##0}</td>
                                            <td style='border-bottom: 1px solid; border-top: 1px solid; text-align: right'>{total_nilai_prm:#,##0}</td>
                                            <td style='border-bottom: 1px solid; border-top: 1px solid; text-align: right'>{total_nilai_kms:#,##0}</td>
                                            <td style='border-bottom: 1px solid; border-top: 1px solid; text-align: right'>{total_nilai_hf:#,##0}</td>
                                            <td style='border-bottom: 1px solid; border-top: 1px solid; text-align: right'>{total_nilai_net:#,##0}</td>
                                        </tr>
                                    </table>
                                    </div>");
                
                total_semua_sum_ins += total_sum_ins;
                total_semua_pst_share += total_pst_share;
                total_semua_nilai_prm += total_nilai_prm;
                total_semua_nilai_kms += total_nilai_kms;
                total_semua_nilai_hf += total_nilai_hf;
                total_semua_nilai_net += total_nilai_net;
            }
            
            resultTemplate = templateProfileResult.Render( new
            {
                total_semua_sum_ins = ReportHelper.ConvertToReportFormat(total_semua_sum_ins), 
                total_semua_share_member = ReportHelper.ConvertToReportFormat(total_semua_pst_share), 
                total_semua_premi_koas = ReportHelper.ConvertToReportFormat(total_semua_nilai_prm),
                total_semua_commission = ReportHelper.ConvertToReportFormat(total_semua_nilai_kms), 
                total_semua_handling_fee = ReportHelper.ConvertToReportFormat(total_semua_nilai_hf), 
                total_semua_amount_due_to_you = ReportHelper.ConvertToReportFormat(total_semua_nilai_net),
                details = stringBuilder.ToString()
            } );
            
            return resultTemplate;
        }
    }
}