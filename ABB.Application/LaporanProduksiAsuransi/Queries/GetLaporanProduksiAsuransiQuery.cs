using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;
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
                                $"{request.kd_cb.Trim()},{request.kd_grp_sb_bis.Trim()},{request.kd_rk_sb_bis.Trim()},{request.kd_cob.Trim()}" +
                                $"{request.kd_scob.Trim()},{prm_th},{request.kd_grp_ttg.Trim()},{request.kd_rk_ttg.Trim()}" +
                                $"{request.kd_grp_mkt.Trim()},{request.kd_rk_mkt.Trim()}"
                })).ToList();

            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "LaporanProduksiAsuransi.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (laporanProduksiAsuransiDatas.Count == 0)
            {
                throw new NullReferenceException("Data tidak ditemukan");
            }
            
            Template templateProfileResult = Template.Parse( templateReportHtml );

            var nama_cobs = laporanProduksiAsuransiDatas.Select(s => s.nm_cob).Distinct().ToList();

            string resultTemplate;
            

            StringBuilder stringBuilder = new StringBuilder();
            decimal total_semua_nilai_prm = 0;
            decimal total_semua_nilai_dis = 0;
            decimal total_semua_nilai_kms = 0;
            decimal total_semua_premi_netto = 0;
            decimal total_semua_nilai_bia_pol = 0;
            decimal total_semua_nilai_bia_mat = 0;
            decimal total_semua_group = 0;
            foreach (var nama_cob in nama_cobs)
            {
                var sequence = 0;
                var laporanProduksiAsuransi = laporanProduksiAsuransiDatas.FirstOrDefault(w => w.nm_cob == nama_cob);
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
                                                        <td style='vertical-align: top; text-align: justify;'>:</td>
                                                        <td style='vertical-align: top; text-align: justify;'>{laporanProduksiAsuransi?.tgl_period}</td>
                                                    </tr>
                                                    <tr>
                                                        <td style='width: 30%;'></td>
                                                        <td style='width: 20%; text-align: left;'>CABANG PERUSAHAAN</td>
                                                        <td style='vertical-align: top; text-align: justify; width: 1%;'>:</td>
                                                        <td style='vertical-align: top; text-align: justify; width: 70%;'>{laporanProduksiAsuransi?.nm_cb}</td>
                                                    </tr>
                                                    <tr>
                                                        <td style='width: 30%;'></td>
                                                        <td style='width: 20%; text-align: left;'>JENIS BISNIS</td>
                                                        <td style='vertical-align: top; text-align: justify; width: 1%;'>:</td>
                                                        <td style='vertical-align: top; text-align: justify; width: 70%;'>{laporanProduksiAsuransi?.nm_cob}</td>
                                                    </tr>
                                                    <tr>
                                                        <td style='width: 30%;'></td>
                                                        <td style='width: 20%; text-align: left;'>SUMBER BISNIS</td>
                                                        <td style='vertical-align: top; text-align: justify; width: 1%;'>:</td>
                                                        <td style='vertical-align: top; text-align: justify; width: 70%;'>{laporanProduksiAsuransi?.nm_sb_bis}</td>
                                                    </tr>
                                                    <tr>
                                                        <td style='width: 30%;'></td>
                                                        <td style='width: 20%; text-align: left;'>MARKETING</td>
                                                        <td style='vertical-align: top; text-align: justify; width: 1%;'>:</td>
                                                        <td style='vertical-align: top; text-align: justify; width: 70%;'>{laporanProduksiAsuransi?.nm_mkt}</td>
                                                    </tr>
                                                </table>
                                                <table class='table'>
                                                    <tr>
                                                        <td style='width: 3%; text-align: center; border: 1px solid' rowspan='2'>NOMOR URUT</td>
                                                        <td style='width: 20%; text-align: center; border: 1px solid' rowspan='2'>NOMOR POLIS<br>NOMOR NOTA</td>
                                                        <td style='width: 20%; text-align: center; border: 1px solid' rowspan='2'>NAMA  TERTANGGUNG<br>QQ</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid' rowspan='2'>PERIODE PERTANGGUNGAN<br>TANGGAL NOTA</td>
                                                        <td style='width: 5%;  text-align: center; border: 1px solid' rowspan='2'>TSI<br>NO. REG</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid' rowspan='2'>TAHUN</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid' rowspan='2'>CURRENCY</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid' rowspan='2'>NILAI PREMI</td>
                                                        <td style='vertical-align: top; text-align: center; border: 1px solid'>DISKON</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid' rowspan='2'>PREMI NETTO</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid' rowspan='2'>BIAYA POLIS/<br>ENDORSMENT</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid' rowspan='2'>BIAYA METERAI</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid' rowspan='2'>JUMLAH</td>
                                                    </tr>
                                                    <tr>
                                                        <td style='vertical-align: top; text-align: center; border: 1px solid'>KOMISI</td>
                                                    </tr>
                                                    <tr style='height: 50px;'>
                                                        <td style='font-weight: bold; border: 1px solid' colspan='13'>{nama_cob}</td>
                                                    </tr>");
                decimal total_nilai_prm = 0;
                decimal total_nilai_dis = 0;
                decimal total_nilai_kms = 0;
                decimal total_premi_netto = 0;
                decimal total_nilai_bia_pol = 0;
                decimal total_nilai_bia_mat = 0;
                decimal total_group = 0;
                foreach (var data in laporanProduksiAsuransiDatas.Where(w => w.nm_cob == nama_cob))
                {
                    sequence++;
                    var nilai_ttl_ptg = data.nilai_ttl_ptg == null ? "0" : data.nilai_ttl_ptg.Value.ToString("#,##0");
                    var nilai_dis = data.nilai_dis == null ? "0" : data.nilai_dis.Value.ToString("#,##0");
                    var nilai_prm = data.nilai_prm == null ? "0" : data.nilai_prm.Value.ToString("#,##0");
                    var nilai_kms = data.nilai_kms == null ? "0" : data.nilai_kms.Value.ToString("#,##0");
                    var nilai_bia_pol = data.nilai_ttl_ptg == null ? "0" : data.nilai_bia_pol.Value.ToString("#,##0");
                    var nilai_bia_mat = data.nilai_ttl_ptg == null ? "0" : data.nilai_bia_mat.Value.ToString("#,##0");
                    var premi_netto = Convert.ToDecimal(data.nilai_prm) - (Convert.ToDecimal(data.nilai_dis) + Convert.ToDecimal(data.nilai_kms));
                    var total_bia = Convert.ToDecimal(data.nilai_bia_pol) + Convert.ToDecimal(data.nilai_bia_mat);
                    stringBuilder.Append(@$"<tr>
                                                <td style='width: 3%;  text-align: left; vertical-align: top; border: 1px solid'>{sequence}</td>
                                                <td style='width: 20%; text-align: left; vertical-align: top; border: 1px solid'>{data.no_pol_ttg}<br>{data.no_nota}</td>
                                                <td style='width: 20%; text-align: left; vertical-align: top; border: 1px solid'>{data.nm_ttg}<br>{data.nm_qq}</td>
                                                <td style='width: 10%; text-align: center; vertical-align: top; border: 1px solid'>{data.tgl_mul_ptg_ind} s/d {data.tgl_akh_ptg_ind}<br>{data.tgl_nt.Value.ToShortDateString()}</td>
                                                <td style='width: 5%;  text-align: right; vertical-align: top; border: 1px solid'>{nilai_ttl_ptg}<br>{data.no_reg}</td>
                                                <td style='width: 10%; text-align: left; vertical-align: top; border: 1px solid'>{data.kd_cvrg}</td>
                                                <td style='width: 10%; text-align: center; vertical-align: top; border: 1px solid'>Rp.</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{nilai_prm}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{nilai_dis}<br>{nilai_kms}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{premi_netto}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{nilai_bia_pol}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{nilai_bia_mat}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{total_bia}</td>
                                            </tr>");
                    total_nilai_prm += Convert.ToDecimal(nilai_ttl_ptg);
                    total_nilai_dis += Convert.ToDecimal(nilai_dis);
                    total_nilai_kms += Convert.ToDecimal(nilai_kms);
                    total_premi_netto += premi_netto;
                    total_nilai_bia_pol += Convert.ToDecimal(nilai_bia_pol);
                    total_nilai_bia_mat += Convert.ToDecimal(nilai_bia_mat);
                    total_group += total_bia;
                }

                stringBuilder.Append(@$"<tr>
                                            <td colspan=1></td>
                                        </tr>
                                        <tr>
                                            <td colspan=6 style='border-bottom: 1px solid; border-top: 1px solid'>TOTAL DALAM ORIGINAL</td>
                                            <td style='border-bottom: 1px solid; border-top: 1px solid; text-align: center'>Rp.</td>
                                            <td style='border-bottom: 1px solid; border-top: 1px solid; text-align: right'>{total_nilai_prm:#,##0}</td>
                                            <td style='border-bottom: 1px solid; border-top: 1px solid; text-align: right'>{total_nilai_dis:#,##0}<br>{total_nilai_kms:#,##0}</td>
                                            <td style='border-bottom: 1px solid; border-top: 1px solid; text-align: right'>{total_premi_netto:#,##0}</td>
                                            <td style='border-bottom: 1px solid; border-top: 1px solid; text-align: right'>{total_nilai_bia_pol:#,##0}</td>
                                            <td style='border-bottom: 1px solid; border-top: 1px solid; text-align: right'>{total_nilai_bia_mat:#,##0}</td>
                                            <td style='border-bottom: 1px solid; border-top: 1px solid; text-align: right'>{total_group:#,##0}</td>
                                        </tr>
                                    </table>
                                    </div>");
                
                total_semua_nilai_prm += total_nilai_prm;
                total_semua_nilai_dis += total_nilai_dis;
                total_semua_nilai_kms += total_nilai_kms;
                total_semua_premi_netto += total_premi_netto;
                total_semua_nilai_bia_pol += total_nilai_bia_pol;
                total_semua_nilai_bia_mat += total_nilai_bia_mat;
                total_semua_group += total_group;
            }
            
            resultTemplate = templateProfileResult.Render( new
            {
                total_semua_nilai_prm = total_semua_nilai_prm.ToString("#,##0"), 
                total_semua_nilai_dis = total_semua_nilai_dis.ToString("#,##0"), 
                total_semua_nilai_kms = total_semua_nilai_kms.ToString("#,##0"),
                total_semua_premi_netto = total_semua_premi_netto.ToString("#,##0"), 
                total_semua_nilai_bia_pol = total_semua_nilai_bia_pol.ToString("#,##0"), 
                total_semua_nilai_bia_mat = total_semua_nilai_bia_mat.ToString("#,##0"),
                total_semua_group = total_semua_group.ToString("#,##0"),
                laporanProduksiAsuransiDatas[0].nm_cb,
                date = DateTime.Now.ToString("dd MMMM yyyy"), details = stringBuilder.ToString()
            } );
            
            return resultTemplate;
        }
    }
}