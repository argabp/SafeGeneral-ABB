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

namespace ABB.Application.LaporanProduksiAsuransiMasuk.Queries
{
    public class GetLaporanProduksiAsuransiMasukQuery : IRequest<string>
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

    public class GetLaporanProduksiAsuransiMasukQueryHandler : IRequestHandler<GetLaporanProduksiAsuransiMasukQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;

        public GetLaporanProduksiAsuransiMasukQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
        }

        public async Task<string> Handle(GetLaporanProduksiAsuransiMasukQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            var kd_thn = request.kd_thn == null ? string.Empty : request.kd_thn.Trim();
            var laporanProduksiAsuransiMasukDatas = (await _connectionFactory.QueryProc<LaporanProduksiAsuransiMasukDto>("spr_uw05r_01", 
                new
                {
                    input_str = $"{request.kd_bln_mul.ToShortDateString()},{request.kd_bln_akh.ToShortDateString()},{kd_thn}," +
                                $"{request.kd_cb.Trim()},{request.kd_cob.Trim()},{request.kd_scob.Trim()}," +
                                $"{request.kd_grp_sb_bis.Trim()},{request.kd_rk_sb_bis.Trim()}"
                })).ToList();

            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "LaporanProduksiAsuransiMasuk.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (laporanProduksiAsuransiMasukDatas.Count == 0)
            {
                throw new NullReferenceException("Data tidak ditemukan");
            }
            
            Template templateProfileResult = Template.Parse( templateReportHtml );

            var nama_cobs = laporanProduksiAsuransiMasukDatas.Select(s => s.nm_cob).Distinct().ToList();

            string resultTemplate;
            
            StringBuilder stringBuilder = new StringBuilder();
            decimal total_semua_sum_ins = 0;
            decimal total_semua_gross_premium = 0;
            decimal total_semua_commision = 0;
            decimal total_semua_discount = 0;
            decimal total_semua_handling_fee = 0;
            decimal total_semua_net_due_to_us = 0;
            foreach (var nama_cob in nama_cobs)
            {
                var sequence = 0;
                var laporanProduksiAsuransimasuk = laporanProduksiAsuransiMasukDatas.FirstOrDefault(w => w.nm_cob == nama_cob);
                stringBuilder.Append(@$"<div style='page-break-before: always;'>
                                        <p style='font-size: 14px; margin: auto;'><strong>PT. ASURANSI BHAKTI BHAYANGKARA</strong></p>

                                        <div class='container'>
                                            <div class='section'>
                                                <p style='font-size: 14px; margin: auto; text-align: center;'>BUKU PRODUKSI KOASURANSI MASUK</p>
                                                <p style='font-size: 14px; margin: auto; text-align: center;'>JENIS PERTANGGUNGAN : {laporanProduksiAsuransimasuk.nm_cob}</p>
                                                <p style='font-size: 14px; margin: auto; text-align: center;'>PERIODE: {laporanProduksiAsuransimasuk.periode}</p>
                                                <table class='table'>
                                                    <tr>
                                                        <td style='width: 3%; text-align: center; border: 1px solid'>NO</td>
                                                        <td style='width: 20%; text-align: center; border: 1px solid'>NOMOR/TANGGAL NOTA</td>
                                                        <td style='width: 20%; text-align: center; border: 1px solid'>CEDING COMPANY</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid'>NAMA TERTANGGUNG / LOKET</td>
                                                        <td style='width: 5%;  text-align: center; border: 1px solid'>NOMOR POLIS</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid'>TOTAL SUM INSURED</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid'>GROSS PREMIUM</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid'>COMISSION</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid'>DISCOUNT</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid'>H. FEE</td>
                                                        <td style='width: 10%; text-align: center; border: 1px solid'>NET. DUE TO US</td>
                                                    </tr>");
                decimal total_sum_ins = 0;
                decimal total_gross_premium = 0;
                decimal total_commision = 0;
                decimal total_discount = 0;
                decimal total_handling_fee = 0;
                decimal total_net_due_to_us = 0;
                foreach (var data in laporanProduksiAsuransiMasukDatas.Where(w => w.nm_cob == nama_cob))
                {
                    sequence++;
                    var nilai_ttl_ptg = MoneyHelper.ConvertToReportFormat(data.nilai_ttl_ptg) == null ? "0" : data.nilai_ttl_ptg.Value.ToString("#,##0");
                    var nilai_prm = MoneyHelper.ConvertToReportFormat(data.nilai_prm) == null ? "0" : data.nilai_prm.Value.ToString("#,##0");
                    var nilai_kms = MoneyHelper.ConvertToReportFormat(data.nilai_kms) == null ? "0" : data.nilai_kms.Value.ToString("#,##0");
                    var nilai_dis = MoneyHelper.ConvertToReportFormat(data.nilai_dis) == null ? "0" : data.nilai_dis.Value.ToString("#,##0");
                    var nilai_hf = MoneyHelper.ConvertToReportFormat(data.nilai_ttl_ptg) == null ? "0" : data.nilai_hf.Value.ToString("#,##0");
                    var nilai_net = MoneyHelper.ConvertToReportFormat(data.nilai_ttl_ptg) == null ? "0" : data.nilai_net.Value.ToString("#,##0");
                    stringBuilder.Append(@$"<tr>
                                                <td style='width: 3%;  text-align: left; vertical-align: top; border: 1px solid'>{sequence}</td>
                                                <td style='width: 20%; text-align: left; vertical-align: top; border: 1px solid'>{data.no_nota}<br>{data.tgl_nt}</td>
                                                <td style='width: 20%; text-align: left; vertical-align: top; border: 1px solid'>{data.nm_pas}</td>
                                                <td style='width: 20%; text-align: left; vertical-align: top; border: 1px solid'>{data.nm_ttg}</td>
                                                <td style='width: 10%; text-align: center; vertical-align: top; border: 1px solid'>{data.no_pol_ttg}</td>
                                                <td style='width: 5%;  text-align: right; vertical-align: top; border: 1px solid'>{nilai_ttl_ptg}</td>
                                                <td style='width: 10%; text-align: left; vertical-align: top; border: 1px solid'>{nilai_prm}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{nilai_kms}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{nilai_dis}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{nilai_hf}</td>
                                                <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{nilai_net}</td>
                                            </tr>");
                    total_sum_ins += MoneyHelper.ConvertToDecimalFormat(nilai_ttl_ptg);
                    total_gross_premium += MoneyHelper.ConvertToDecimalFormat(nilai_prm);
                    total_commision += MoneyHelper.ConvertToDecimalFormat(nilai_kms);
                    total_discount += MoneyHelper.ConvertToDecimalFormat(nilai_dis);
                    total_handling_fee += MoneyHelper.ConvertToDecimalFormat(nilai_hf);
                    total_net_due_to_us += MoneyHelper.ConvertToDecimalFormat(nilai_net);
                }

                stringBuilder.Append(@$"<tr>
                                            <td colspan=1></td>
                                        </tr>
                                        <tr>
                                            <td colspan=5 style='border-bottom: 1px solid; border-top: 1px solid'>Sub Total Nilai</td>
                                            <td style='border-bottom: 1px solid; border-top: 1px solid; text-align: right'>{total_sum_ins:#,##0}</td>
                                            <td style='border-bottom: 1px solid; border-top: 1px solid; text-align: right'>{total_gross_premium:#,##0}</td>
                                            <td style='border-bottom: 1px solid; border-top: 1px solid; text-align: right'>{total_commision:#,##0}</td>
                                            <td style='border-bottom: 1px solid; border-top: 1px solid; text-align: right'>{total_discount:#,##0}</td>
                                            <td style='border-bottom: 1px solid; border-top: 1px solid; text-align: right'>{total_handling_fee:#,##0}</td>
                                            <td style='border-bottom: 1px solid; border-top: 1px solid; text-align: right'>{total_net_due_to_us:#,##0}</td>
                                        </tr>
                                    </table>
                                    </div>");
                
                total_semua_sum_ins += total_sum_ins;
                total_semua_gross_premium += total_gross_premium;
                total_semua_commision += total_commision;
                total_semua_discount += total_discount;
                total_semua_handling_fee += total_handling_fee;
                total_semua_net_due_to_us += total_net_due_to_us;
            }
            
            resultTemplate = templateProfileResult.Render( new
            {
                total_semua_sum_ins = MoneyHelper.ConvertToReportFormat(total_semua_sum_ins), 
                total_semua_gross_premium = MoneyHelper.ConvertToReportFormat(total_semua_gross_premium), 
                total_semua_commision = MoneyHelper.ConvertToReportFormat(total_semua_commision),
                total_semua_discount = MoneyHelper.ConvertToReportFormat(total_semua_discount), 
                total_semua_handling_fee = MoneyHelper.ConvertToReportFormat(total_semua_handling_fee), 
                total_semua_net_due_to_us = MoneyHelper.ConvertToReportFormat(total_semua_net_due_to_us),
                details = stringBuilder.ToString()
            } );
            
            return resultTemplate;
        }
    }
}