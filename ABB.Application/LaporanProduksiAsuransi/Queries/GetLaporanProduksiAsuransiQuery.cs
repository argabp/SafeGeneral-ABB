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
using Newtonsoft.Json;
using Scriban;

namespace ABB.Application.LaporanProduksiAsuransi.Queries
{
    public class GetLaporanProduksiAsuransiQuery : IRequest<string>
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
    }

    public class GetLaporanProduksiAsuransiQueryHandler : IRequestHandler<GetLaporanProduksiAsuransiQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;

        public GetLaporanProduksiAsuransiQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
        }

        public async Task<string> Handle(GetLaporanProduksiAsuransiQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            var kd_thn = request.kd_thn == null ? string.Empty : request.kd_thn.Trim();
            var prm_th = request.prm_th == null ? string.Empty : request.prm_th.Trim();
            var laporanProduksiAsuransiDatas = (await _connectionFactory.QueryProc<LaporanProduksiAsuransiDto>("spr_uw04r_01", 
                new
                {
                    input_str = $"{request.kd_bln_mul.ToShortDateString()},{request.kd_bln_akh.ToShortDateString()},{kd_thn}," +
                                $"{request.kd_cb.Trim()},{request.kd_grp_sb_bis.Trim()},{request.kd_cob.Trim()}," +
                                $"{request.kd_scob.Trim()},{prm_th},{request.kd_rk_sb_bis.Trim()},{request.kd_grp_ttg.Trim()},{request.kd_rk_ttg.Trim()}," +
                                $"{request.kd_grp_mkt.Trim()},{request.kd_rk_mkt.Trim()}"
                })).ToList();

            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "LaporanProduksiAsuransi.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (laporanProduksiAsuransiDatas.Count == 0)
            {
                throw new NullReferenceException("Data tidak ditemukan");
            }
            
            Template templateProfileResult = Template.Parse( templateReportHtml );

            string resultTemplate;
            
            decimal grand_total_nilai_prm = 0;
            decimal grand_total_nilai_prm_idr = 0;
            decimal grand_total_nilai_dis = 0;
            decimal grand_total_nilai_dis_idr = 0;
            decimal grand_total_nilai_kms = 0;
            decimal grand_total_nilai_kms_idr = 0;
            decimal grand_total_premi_netto = 0;
            decimal grand_total_premi_netto_idr = 0;
            decimal grand_total_nilai_bia_pol = 0;
            decimal grand_total_nilai_bia_pol_idr = 0;
            decimal grand_total_nilai_bia_mat = 0;
            decimal grand_total_nilai_bia_mat_idr = 0;
            decimal grand_total_total_group = 0;
            decimal grand_total_total_group_idr = 0;
            
            var groupedData = laporanProduksiAsuransiDatas
            .GroupBy(x => new { x.kd_grp_sb_bis, x.kd_rk_sb_bis, x.kd_bln, x.kd_cob }) // Outer group
            .ToList();

            var lastOuterKey = groupedData.Last().Key;
            
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var outerGroup in groupedData)
            {
                // Outer group key
                var outerFirst = outerGroup.FirstOrDefault();

                // 💡 Now group by kd_mkt INSIDE the outer group
                var innerGroups = outerGroup.GroupBy(x => x.kd_mtu).ToList();
                
                decimal total_outer_nilai_prm = 0;
                decimal total_outer_nilai_prm_idr = 0;
                decimal total_outer_nilai_dis = 0;
                decimal total_outer_nilai_dis_idr = 0;
                decimal total_outer_nilai_kms = 0;
                decimal total_outer_nilai_kms_idr = 0;
                decimal total_outer_premi_netto = 0;
                decimal total_outer_premi_netto_idr = 0;
                decimal total_outer_nilai_bia_pol = 0;
                decimal total_outer_nilai_bia_pol_idr = 0;
                decimal total_outer_nilai_bia_mat = 0;
                decimal total_outer_nilai_bia_mat_idr = 0;
                decimal total_outer_group = 0;
                decimal total_outer_group_idr = 0;

                var lastInnerKey = innerGroups.Last().Key;
                
                foreach (var innerGroup in innerGroups)
                {
                    var innerFirst = innerGroup.FirstOrDefault();

                    stringBuilder.Append(@$"<div style='page-break-before: always;'>
                        <p style='font-size: 14px; margin: auto;'><strong>PT. ASURANSI BHAKTI BHAYANGKARA</strong></p>
                        <p style='font-size: 12px; margin: auto;'><strong>Jakarta Selatan</strong></p>
                        <p style='font-size: 12px; margin: auto;'><strong>Jakarta</strong></p>

                        <div class='container'>
                            <div class='section'>
                                <p style='font-size: 14px; margin: auto; text-align: center;'><strong>LAPORAN PRODUKSI ASURANSI</strong></p>
                                <table style='font-size: 12px; width: 100%'>
                                    <tr>
                                        <td style='width: 30%;'></td>
                                        <td style='width: 20%; text-align: left;'>PERIODE</td>
                                        <td style='vertical-align: top;'>:</td>
                                        <td colspan='2'>{outerFirst?.tgl_period}</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>CABANG PERUSAHAAN</td>
                                        <td>:</td>
                                        <td colspan='2'>{outerFirst?.nm_cb}</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>JENIS BISNIS</td>
                                        <td>:</td>
                                        <td colspan='2'>{outerFirst?.nm_cob}</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>SUMBER BISNIS</td>
                                        <td>:</td>
                                        <td colspan='2'>{outerFirst?.nm_sb_bis}</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>MARKETING</td>
                                        <td>:</td>
                                        <td>{innerFirst?.nm_mkt}</td>
                                        <td style='font-size: 8pt'>Page 1</td>
                                    </tr>
                                </table>

                                <table class='table'>
                                    <tr>
                                        <td rowspan='2' style='width: 3%; text-align: center; border: 1px solid'>NOMOR URUT</td>
                                        <td rowspan='2' style='width: 20%; text-align: center; border: 1px solid'>NOMOR POLIS<br>NOMOR NOTA</td>
                                        <td rowspan='2' style='width: 22%; text-align: center; border: 1px solid'>NAMA  TERTANGGUNG<br>QQ</td>
                                        <td rowspan='2' style='width: 10%; text-align: center; border: 1px solid'>PERIODE PERTANGGUNGAN<br>TANGGAL NOTA</td>
                                        <td rowspan='2' style='width: 5%; text-align: center; border: 1px solid'>TSI<br>NO. REG</td>
                                        <td rowspan='2' style='width: 5%; text-align: center; border: 1px solid'>TAHUN</td>
                                        <td rowspan='2' style='width: 5%; text-align: center; border: 1px solid'>CURRENCY</td>
                                        <td rowspan='2' style='width: 5%; text-align: center; border: 1px solid'>NILAI PREMI</td>
                                        <td style='width:5%; text-align: center; border: 1px solid'>DISKON</td>
                                        <td rowspan='2' style='width: 5%; text-align: center; border: 1px solid'>PREMI NETTO</td>
                                        <td rowspan='2' style='width: 5%; text-align: center; border: 1px solid'>BIAYA POLIS/<br>ENDORSMENT</td>
                                        <td rowspan='2' style='width: 5%; text-align: center; border: 1px solid'>BIAYA METERAI</td>
                                        <td rowspan='2' style='width: 5%; text-align: center; border: 1px solid'>JUMLAH</td>
                                    </tr>
                                    <tr>
                                        <td style='width:5%; text-align: center; border: 1px solid'>KOMISI</td>
                                    </tr>
                                    <tr>
                                        <td colspan='13' style='font-weight: bold; border: 1px solid'>{outerFirst?.nm_cob}</td>
                                    </tr>");

                    // Subtotal variables for inner group
                    int sequence = 0;
                    decimal total_nilai_prm = 0;
                    decimal total_nilai_prm_idr = 0;
                    decimal total_nilai_dis = 0;
                    decimal total_nilai_dis_idr = 0;
                    decimal total_nilai_kms = 0;
                    decimal total_nilai_kms_idr = 0;
                    decimal total_premi_netto = 0;
                    decimal total_premi_netto_idr = 0;
                    decimal total_nilai_bia_pol = 0;
                    decimal total_nilai_bia_pol_idr = 0;
                    decimal total_nilai_bia_mat = 0;
                    decimal total_nilai_bia_mat_idr = 0;
                    decimal total_group = 0;
                    decimal total_group_idr = 0;

                    foreach (var data in innerGroup)
                    {
                        sequence++;

                        var nilai_ttl_ptg = ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, data.nilai_ttl_ptg);
                        var nilai_prm = data.kd_mtu == "001" ? string.Empty : ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, data.nilai_prm);
                        var nilai_prm_idr = ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, data.nilai_prm_idr);
                        var nilai_dis = data.kd_mtu == "001" ? string.Empty : ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, data.nilai_dis);
                        var nilai_dis_idr = ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, data.nilai_dis_idr);
                        var nilai_kms = data.kd_mtu == "001" ? string.Empty : ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, data.nilai_kms);
                        var nilai_kms_idr = ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, data.nilai_kms_idr);
                        var nilai_bia_pol = data.kd_mtu == "001" ? string.Empty : ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, data.nilai_bia_pol);
                        var nilai_bia_pol_idr = ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, data.nilai_bia_pol_idr);
                        var nilai_bia_mat = data.kd_mtu == "001" ? string.Empty : ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, data.nilai_bia_mat);
                        var nilai_bia_mat_idr = ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, data.nilai_bia_mat_idr);
                        var premi_netto = data.kd_mtu == "001" ? string.Empty : ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, Convert.ToDecimal(data.nilai_prm) - (Convert.ToDecimal(data.nilai_dis) + Convert.ToDecimal(data.nilai_kms)));
                        var premi_netto_idr = ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, Convert.ToDecimal(data.nilai_prm_idr) - (Convert.ToDecimal(data.nilai_dis_idr) + Convert.ToDecimal(data.nilai_kms_idr)));
                        var total_bia = data.kd_mtu == "001" ? string.Empty : ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, Convert.ToDecimal(data.nilai_prm) - (Convert.ToDecimal(data.nilai_dis) + Convert.ToDecimal(data.nilai_kms)) + Convert.ToDecimal(data.nilai_bia_pol) + Convert.ToDecimal(data.nilai_bia_mat));
                        var total_bia_idr = ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, Convert.ToDecimal(data.nilai_prm_idr) - (Convert.ToDecimal(data.nilai_dis_idr) + Convert.ToDecimal(data.nilai_kms_idr)) + Convert.ToDecimal(data.nilai_bia_pol_idr) + Convert.ToDecimal(data.nilai_bia_mat_idr));

                        var kd_symbol_mtu = data.kd_mtu == "001" ? string.Empty : data.kd_symbol_mtu;
                        
                        stringBuilder.Append(@$"<tr>
                            <td style='border: 1px solid'>{sequence}</td>
                            <td style='border: 1px solid'>{data.no_pol_ttg}<br>{data.no_nota}</td>
                            <td style='border: 1px solid'>{data.nm_ttg}<br><br>{data.nm_qq}</td>
                            <td style='border: 1px solid'>{data.tgl_mul_ptg_ind} s/d {data.tgl_akh_ptg_ind}<br>{data.tgl_nt?.ToShortDateString()}</td>
                            <td style='border: 1px solid; text-align: right'>{nilai_ttl_ptg}<br><br>{data.no_reg}</td>
                            <td style='border: 1px solid'>{data.kd_cvrg}</td>
                            <td style='border: 1px solid; text-align: center'>{kd_symbol_mtu}<br><br>Rp.</td>
                            <td style='border: 1px solid; text-align: right'>{nilai_prm}<br><br>{nilai_prm_idr}</td>
                            <td style='border: 1px solid; text-align: right'>{nilai_dis}<br>{nilai_kms}<br>{nilai_dis_idr}<br>{nilai_kms_idr}</td>
                            <td style='border: 1px solid; text-align: right'>{premi_netto}<br><br>{premi_netto_idr}</td>
                            <td style='border: 1px solid; text-align: right'>{nilai_bia_pol}<br><br>{nilai_bia_pol_idr}</td>
                            <td style='border: 1px solid; text-align: right'>{nilai_bia_mat}<br><br>{nilai_bia_mat_idr}</td>
                            <td style='border: 1px solid; text-align: right'>{total_bia}<br><br>{total_bia_idr}</td>
                        </tr>");

                        total_nilai_prm += ReportHelper.ConvertToDecimalFormat(nilai_prm);
                        total_nilai_prm_idr += ReportHelper.ConvertToDecimalFormat(nilai_prm_idr);
                        total_nilai_dis += ReportHelper.ConvertToDecimalFormat(nilai_dis);
                        total_nilai_kms += ReportHelper.ConvertToDecimalFormat(nilai_kms);
                        total_premi_netto += ReportHelper.ConvertToDecimalFormat(premi_netto);
                        total_premi_netto_idr += ReportHelper.ConvertToDecimalFormat(premi_netto_idr);
                        total_nilai_bia_pol += ReportHelper.ConvertToDecimalFormat(nilai_bia_pol);
                        total_nilai_bia_pol_idr += ReportHelper.ConvertToDecimalFormat(nilai_bia_pol_idr);
                        total_nilai_bia_mat += ReportHelper.ConvertToDecimalFormat(nilai_bia_mat);
                        total_nilai_bia_mat_idr += ReportHelper.ConvertToDecimalFormat(nilai_bia_mat_idr);
                        total_group += ReportHelper.ConvertToDecimalFormat(total_bia);
                        total_group_idr += ReportHelper.ConvertToDecimalFormat(total_bia_idr);
                    }

                    // ✅ Subtotal per kd_mkt

                    if (innerFirst.kd_mtu != "001")
                    {
                        stringBuilder.Append(@$"<tr>
                            <td colspan='6' style='border-top: 1px solid;'>TOTAL DALAM Original</td>
                            <td style='border-top: 1px solid; text-align: center'>{innerFirst.kd_symbol_mtu}</td>
                            <td style='border-top: 1px solid; text-align: right'>{ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, total_nilai_prm)}</td>
                            <td style='border-top: 1px solid; text-align: right'>{ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, total_nilai_dis)}</td>
                            <td style='border-top: 1px solid; text-align: right'>{ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, total_premi_netto)}</td>
                            <td style='border-top: 1px solid; text-align: right'>{ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, total_nilai_bia_pol)}</td>
                            <td style='border-top: 1px solid; text-align: right'>{ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, total_nilai_bia_mat)}</td>
                            <td style='border-top: 1px solid; text-align: right'>{ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, total_group)}</td>
                        </tr>");
                    
                        stringBuilder.Append(@$"<tr>
                        <td colspan='6' style='border-bottom: 1px solid;'></td>
                        <td style='border-bottom: 1px solid; text-align: center'>{innerFirst.kd_symbol_mtu}</td>
                        <td style='border-bottom: 1px solid; text-align: right'></td>
                        <td style='border-bottom: 1px solid; text-align: right'>{ReportHelper.ConvertToReportFormat(innerFirst.kd_symbol_mtu, total_nilai_kms)}</td>
                        <td style='border-bottom: 1px solid; text-align: right'></td>
                        <td style='border-bottom: 1px solid; text-align: right'></td>
                        <td style='border-bottom: 1px solid; text-align: right'></td>
                        <td style='border-bottom: 1px solid; text-align: right'></td>
                    </tr>");
                    }

                    // ✅ Subtotal per kd_mkt
                    stringBuilder.Append(@$"<tr>
                        <td colspan='6' style='border-top: 1px solid;'>TOTAL DALAM RUPIAH</td>
                        <td style='border-top: 1px solid; text-align: center'>Rp.</td>
                        <td style='border-top: 1px solid; text-align: right'>{ReportHelper.ConvertToReportFormat(total_nilai_prm_idr)}</td>
                        <td style='border-top: 1px solid; text-align: right'>{ReportHelper.ConvertToReportFormat(total_nilai_dis_idr)}</td>
                        <td style='border-top: 1px solid; text-align: right'>{ReportHelper.ConvertToReportFormat(total_premi_netto_idr)}</td>
                        <td style='border-top: 1px solid; text-align: right'>{ReportHelper.ConvertToReportFormat(total_nilai_bia_pol_idr)}</td>
                        <td style='border-top: 1px solid; text-align: right'>{ReportHelper.ConvertToReportFormat(total_nilai_bia_mat_idr)}</td>
                        <td style='border-top: 1px solid; text-align: right'>{ReportHelper.ConvertToReportFormat(total_group_idr)}</td>
                    </tr>");
                    
                    stringBuilder.Append(@$"<tr>
                        <td colspan='6' style='border-bottom: 1px solid;'></td>
                        <td style='border-bottom: 1px solid; text-align: center'>Rp.</td>
                        <td style='border-bottom: 1px solid; text-align: right'></td>
                        <td style='border-bottom: 1px solid; text-align: right'>{ReportHelper.ConvertToReportFormat(total_nilai_kms_idr)}</td>
                        <td style='border-bottom: 1px solid; text-align: right'></td>
                        <td style='border-bottom: 1px solid; text-align: right'></td>
                        <td style='border-bottom: 1px solid; text-align: right'></td>
                        <td style='border-bottom: 1px solid; text-align: right'></td>
                    </tr>");

                    if (!outerGroup.Key.Equals(lastOuterKey) || !innerGroup.Key.Equals(lastInnerKey))
                    {
                        // append - i.e. if outer isn't last OR inner isn't last
                        stringBuilder.Append("</table></div></div>");
                    }

                    // accumulate subtotal to outer total
                    total_outer_nilai_prm += total_nilai_prm;
                    total_outer_nilai_prm_idr += total_nilai_prm_idr;
                    total_outer_nilai_dis += total_nilai_dis;
                    total_outer_nilai_dis_idr += total_nilai_dis_idr;
                    total_outer_nilai_kms += total_nilai_kms;
                    total_outer_nilai_kms_idr += total_nilai_kms_idr;
                    total_outer_premi_netto += total_premi_netto;
                    total_outer_premi_netto_idr += total_premi_netto_idr;
                    total_outer_nilai_bia_pol += total_nilai_bia_pol;
                    total_outer_nilai_bia_pol_idr += total_nilai_bia_pol_idr;
                    total_outer_nilai_bia_mat += total_nilai_bia_mat;
                    total_outer_nilai_bia_mat_idr += total_nilai_bia_mat_idr;
                    total_outer_group += total_group;
                    total_outer_group_idr += total_group_idr;
                }

                // ✅ Outer group total
                grand_total_nilai_prm += total_outer_nilai_prm;
                grand_total_nilai_prm_idr += total_outer_nilai_prm_idr;
                grand_total_nilai_dis += total_outer_nilai_dis;
                grand_total_nilai_dis_idr += total_outer_nilai_dis_idr;
                grand_total_nilai_kms += total_outer_nilai_kms;
                grand_total_nilai_kms_idr += total_outer_nilai_kms_idr;
                grand_total_premi_netto += total_outer_premi_netto;
                grand_total_premi_netto_idr += total_outer_premi_netto_idr;
                grand_total_nilai_bia_pol += total_outer_nilai_bia_pol;
                grand_total_nilai_bia_pol_idr += total_outer_nilai_bia_pol_idr;
                grand_total_nilai_bia_mat += total_outer_nilai_bia_mat;
                grand_total_nilai_bia_mat_idr += total_outer_nilai_bia_mat_idr;
                grand_total_total_group += total_outer_group;
                grand_total_total_group_idr += total_outer_group_idr;

                if (outerGroup.Key.Equals(lastOuterKey))
                {
                        stringBuilder.Append($@"<tr>
                        <td colspan=6 style='border-bottom: 1px solid; width: 60%'>TOTAL KESELURUHAN DALAM RUPIAH</td>
                        <td style='border-bottom: 1px solid;text-align: right'></td>
                        <td style='border-bottom: 1px solid;text-align: right'>{ReportHelper.ConvertToReportFormat(grand_total_nilai_prm_idr)}</td>
                        <td style='border-bottom: 1px solid;text-align: right'>{ReportHelper.ConvertToReportFormat(grand_total_nilai_dis_idr)}</td>
                        <td style='border-bottom: 1px solid;text-align: right'>{ReportHelper.ConvertToReportFormat(grand_total_premi_netto_idr)}</td>
                        <td style='border-bottom: 1px solid;text-align: right'>{ReportHelper.ConvertToReportFormat(grand_total_nilai_bia_pol_idr)}</td>
                        <td style='border-bottom: 1px solid;text-align: right'>{ReportHelper.ConvertToReportFormat(grand_total_nilai_bia_mat_idr)}</td>
                        <td style='border-bottom: 1px solid;text-align: right'>{ReportHelper.ConvertToReportFormat(grand_total_total_group_idr)}</td>
                    </tr>
                    <tr>
                        <td colspan=6 style='border-bottom: 1px solid; width: 60%'></td>
                        <td style='border-bottom: 1px solid;text-align: right;'></td>
                        <td style='border-bottom: 1px solid;text-align: right;'></td>
                        <td style='border-bottom: 1px solid;text-align: right;'>{ReportHelper.ConvertToReportFormat(grand_total_nilai_kms_idr)}</td>
                        <td style='border-bottom: 1px solid;text-align: right;'></td>
                        <td style='border-bottom: 1px solid;text-align: right;'></td>
                        <td style='border-bottom: 1px solid;text-align: right;'></td>
                        <td style='border-bottom: 1px solid;text-align: right;'></td>
                    </tr>
                    <tr>
                        <td colspan=10></td>
                        <td colspan=3 style='text-align: center'><strong>Departement Underwriting</strong></td>
                    </tr>
                    <tr>
                        <td colspan=10></td>
                        <td colspan=3 style='text-align: center'>{laporanProduksiAsuransiDatas[0].nm_cb}, {DateTime.Now:dd MMMM yyyy}</td>
                    </tr></table></div></div>");
                }
            }
            
            resultTemplate = templateProfileResult.Render( new
            {
                details = stringBuilder.ToString()
            } );
            
            return resultTemplate;
        }
    }
}