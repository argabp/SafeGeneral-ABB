using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.LaporanProduksiAsuransiKeluar.Queries;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Scriban;

namespace ABB.Application.RekapitulasiProduksi.Quries
{
    public class GetRekapitulasiProduksiQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public DateTime kd_bln_mul { get; set; }
        public DateTime kd_bln_akh { get; set; }
        
        public string jns_lap { get; set; }

        public string kd_grp_sb_bis { get; set; }
        public string kd_rk_sb_bis { get; set; }
    }

    public class GetRekapitulasiProduksiQueryHandler : IRequestHandler<GetRekapitulasiProduksiQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;

        public GetRekapitulasiProduksiQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
        }

        public async Task<string> Handle(GetRekapitulasiProduksiQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            var prm_thn = 0;
            var rekapitulasiProduksiDatas = (await _connectionFactory.QueryProc<RekapitulasiProduksiDto>("spr_uw07r_04", 
                new
                {
                    input_str = $"{request.kd_bln_mul:yyyy/MM/dd},{request.kd_bln_akh:yyyy/MM/dd},{request.kd_cb.Trim()}," +
                                $"{prm_thn},{request.jns_lap.Trim()},{request.kd_grp_sb_bis.Trim()},{request.kd_rk_sb_bis.Trim()}"
                })).ToList();

            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "RekapitulasiProduksi.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (rekapitulasiProduksiDatas.Count == 0)
            {
                throw new NullReferenceException("Data tidak ditemukan");
            }
            
            Template templateProfileResult = Template.Parse( templateReportHtml );

            string resultTemplate;

            StringBuilder stringBuilder = new StringBuilder();

            decimal total_premi;
            decimal total_diskon;
            decimal total_komisi;
            decimal total_biaya_polis;
            decimal total_biaya_materai;
            decimal total_brokerage;
            decimal total_net_premi;
            decimal total_polis;
            int sequence;
            switch (request.jns_lap)
            {
                case "7":
                    stringBuilder.Append(@"<table class='table'>
                                    <tr>
                                        <td style='width: 3%; text-align: center; border: 1px solid' colspan=2>BISNIS</td>
                                        <td style='width: 20%; text-align: center; border: 1px solid'>PREMI</td>
                                        <td style='width: 20%; text-align: center; border: 1px solid'>DISKON</td>
                                        <td style='width: 20%; text-align: center; border: 1px solid'>KOMISI</td>
                                        <td style='width: 10%; text-align: center; border: 1px solid'>BIAYA POLIS</td>
                                        <td style='width: 5%;  text-align: center; border: 1px solid'>BIAYA MATERAI</td>
                                        <td style='width: 10%; text-align: center; border: 1px solid'>BROKERAGE</td>
                                        <td style='width: 10%; text-align: center; border: 1px solid'>NET PREMI</td>
                                        <td style='width: 10%; text-align: center; border: 1px solid'>POLIS</td>
                                    </tr>
                                    <tr>
                                        <td colspan='10' style='border: 1px solid'></td>
                                    </tr>");
            
                    total_premi = 0;
                    total_diskon = 0;
                    total_komisi = 0;
                    total_biaya_polis = 0;
                    total_biaya_materai = 0;
                    total_brokerage = 0;
                    total_net_premi = 0;
                    total_polis = 0;
            
                    sequence = 0;
                    
                    foreach (var data in rekapitulasiProduksiDatas)
                    {
                        sequence++;
                        var nilai_prm = MoneyHelper.ConvertToReportFormat(data.nilai_prm);
                        var nilai_diskon = MoneyHelper.ConvertToReportFormat(data.nilai_diskon);
                        var nilai_kms = MoneyHelper.ConvertToReportFormat(data.nilai_kms);
                        var nilai_bia_pol = MoneyHelper.ConvertToReportFormat(data.nilai_bia_pol);
                        var nilai_bia_mat = MoneyHelper.ConvertToReportFormat(data.nilai_bia_mat);
                        var nilai_kms_broker = MoneyHelper.ConvertToReportFormat(data.nilai_kms_broker);
                        var nilai_net = MoneyHelper.ConvertToReportFormat(data.nilai_net);
                        
                        stringBuilder.Append(@$"<tr>
                                                    <td style='width: 3%;  text-align: left; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{sequence}</td>
                                                    <td style='width: 20%; text-align: left; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{data.nm_cob}</td>
                                                    <td style='width: 20%; text-align: right; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{nilai_prm}</td>
                                                    <td style='width: 20%; text-align: right; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{nilai_diskon}</td>
                                                    <td style='width: 20%; text-align: right; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{nilai_kms}</td>
                                                    <td style='width: 5%;  text-align: right; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{nilai_bia_pol}</td>
                                                    <td style='width: 10%; text-align: right; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{nilai_bia_mat}</td>
                                                    <td style='width: 10%; text-align: right; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{nilai_kms_broker}</td>
                                                    <td style='width: 10%; text-align: right; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{nilai_net}</td>
                                                    <td style='width: 10%; text-align: right; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{data.jml_polis}</td>
                                                </tr>");

                        total_premi += MoneyHelper.ConvertToDecimalFormat(nilai_prm);
                        total_diskon += MoneyHelper.ConvertToDecimalFormat(nilai_diskon);
                        total_komisi += MoneyHelper.ConvertToDecimalFormat(nilai_kms);
                        total_biaya_polis += MoneyHelper.ConvertToDecimalFormat(nilai_bia_pol);
                        total_biaya_materai += MoneyHelper.ConvertToDecimalFormat(nilai_bia_mat);
                        total_brokerage += MoneyHelper.ConvertToDecimalFormat(nilai_kms_broker);
                        total_net_premi += MoneyHelper.ConvertToDecimalFormat(nilai_net);
                        total_polis += Convert.ToDecimal(data.jml_polis);
                    }
                        
                    stringBuilder.Append($@"
                            <tr>
                                <td colspan=2 style='border: 1px solid; text-align: center'>Jumlah</td>
                                <td style='border: 1px solid; text-align: right'>{total_premi:#,##0}</td>
                                <td style='border: 1px solid; text-align: right'>{total_diskon:#,##0}</td>
                                <td style='border: 1px solid; text-align: right'>{total_komisi:#,##0}</td>
                                <td style='border: 1px solid; text-align: right'>{total_biaya_polis:#,##0}</td>
                                <td style='border: 1px solid; text-align: right'>{total_biaya_materai:#,##0}</td>
                                <td style='border: 1px solid; text-align: right'>{total_brokerage:#,##0}</td>
                                <td style='border: 1px solid; text-align: right'>{total_net_premi:#,##0}</td>
                                <td style='border: 1px solid; text-align: right'>{total_polis}</td>
                            </tr>
                        </table>");
                    break;
                case "8":
                    var jns_nt_msk = rekapitulasiProduksiDatas.Select(s => s.jns_nt_msk).Select(s => s.Split(" ")[0]).Distinct();

                    foreach (var nt_msk in jns_nt_msk)
                    {
                        string nm_nt_msk = string.Empty;
                        switch (nt_msk)
                        {
                            case "F":
                                nm_nt_msk = "Fakultatif Masuk";
                                break;
                            case "K":
                                nm_nt_msk = "Koasuransi Masuk";
                                break;
                            case "D":
                                nm_nt_msk = "Direct";
                                break;
                            case "0":
                                nm_nt_msk = "Komisi/ Brokerage Tambahan";
                                break;
                            case "L":
                                nm_nt_msk = "Koasuransi Keluar";
                                break;
                        }
                        stringBuilder.Append($@"<p style='font-size: 14px;'><strong>{nm_nt_msk}</strong></p>
                                             <table class='table'>
                            <tr>
                            <td style='width: 3%; text-align: center; border: 1px solid' colspan=2>BISNIS</td>
                            <td style='width: 20%; text-align: center; border: 1px solid'>PREMI</td>
                            <td style='width: 20%; text-align: center; border: 1px solid'>DISKON</td>
                            <td style='width: 20%; text-align: center; border: 1px solid'>KOMISI</td>
                            <td style='width: 10%; text-align: center; border: 1px solid'>BIAYA POLIS</td>
                            <td style='width: 5%;  text-align: center; border: 1px solid'>BIAYA MATERAI</td>
                            <td style='width: 10%; text-align: center; border: 1px solid'>BROKERAGE</td>
                            <td style='width: 10%; text-align: center; border: 1px solid'>NET PREMI</td>
                            <td style='width: 10%; text-align: center; border: 1px solid'>POLIS</td>
                            </tr>
                            <tr>
                            <td colspan='10' style='border: 1px solid'></td>
                            </tr>");
            
                        total_premi = 0;
                        total_diskon = 0;
                        total_komisi = 0;
                        total_biaya_polis = 0;
                        total_biaya_materai = 0;
                        total_brokerage = 0;
                        total_net_premi = 0;
                        total_polis = 0;
            
                        sequence = 0;

                        foreach (var data in rekapitulasiProduksiDatas.Where(w => w.jns_nt_msk.StartsWith(nt_msk)))
                        {
                            sequence++;
                            var nilai_prm = MoneyHelper.ConvertToReportFormat(data.nilai_prm);
                            var nilai_diskon = MoneyHelper.ConvertToReportFormat(data.nilai_diskon);
                            var nilai_kms = MoneyHelper.ConvertToReportFormat(data.nilai_kms);
                            var nilai_bia_pol = MoneyHelper.ConvertToReportFormat(data.nilai_bia_pol);
                            var nilai_bia_mat = MoneyHelper.ConvertToReportFormat(data.nilai_bia_mat);
                            var nilai_kms_broker = MoneyHelper.ConvertToReportFormat(data.nilai_kms_broker);
                            var nilai_net = MoneyHelper.ConvertToReportFormat(data.nilai_net);
                            
                            stringBuilder.Append(@$"<tr>
                                                        <td style='width: 3%;  text-align: left; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{sequence}</td>
                                                        <td style='width: 20%; text-align: left; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{data.nm_cob}</td>
                                                        <td style='width: 20%; text-align: right; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{nilai_prm}</td>
                                                        <td style='width: 20%; text-align: right; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{nilai_diskon}</td>
                                                        <td style='width: 20%; text-align: right; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{nilai_kms}</td>
                                                        <td style='width: 5%;  text-align: right; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{nilai_bia_pol}</td>
                                                        <td style='width: 10%; text-align: right; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{nilai_bia_mat}</td>
                                                        <td style='width: 10%; text-align: right; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{nilai_kms_broker}</td>
                                                        <td style='width: 10%; text-align: right; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{nilai_net}</td>
                                                        <td style='width: 10%; text-align: right; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{data.jml_polis}</td>
                                                    </tr>");
                            
                            total_premi += MoneyHelper.ConvertToDecimalFormat(nilai_prm);
                            total_diskon += MoneyHelper.ConvertToDecimalFormat(nilai_diskon);
                            total_komisi += MoneyHelper.ConvertToDecimalFormat(nilai_kms);
                            total_biaya_polis += MoneyHelper.ConvertToDecimalFormat(nilai_bia_pol);
                            total_biaya_materai += MoneyHelper.ConvertToDecimalFormat(nilai_bia_mat);
                            total_brokerage += MoneyHelper.ConvertToDecimalFormat(nilai_kms_broker);
                            total_net_premi += MoneyHelper.ConvertToDecimalFormat(nilai_net);
                            total_polis += Convert.ToDecimal(data.jml_polis);
                        }
                        
                        stringBuilder.Append($@"
                            <tr>
                                <td colspan=2 style='border: 1px solid; text-align: center'>Jumlah</td>
                                <td style='border: 1px solid; text-align: right'>{total_premi:#,##0}</td>
                                <td style='border: 1px solid; text-align: right'>{total_diskon:#,##0}</td>
                                <td style='border: 1px solid; text-align: right'>{total_komisi:#,##0}</td>
                                <td style='border: 1px solid; text-align: right'>{total_biaya_polis:#,##0}</td>
                                <td style='border: 1px solid; text-align: right'>{total_biaya_materai:#,##0}</td>
                                <td style='border: 1px solid; text-align: right'>{total_brokerage:#,##0}</td>
                                <td style='border: 1px solid; text-align: right'>{total_net_premi:#,##0}</td>
                                <td style='border: 1px solid; text-align: right'>{total_polis}</td>
                            </tr>
                        </table>");
                    }
                    break;
            }
            
            var rekapitulasiProduksi = rekapitulasiProduksiDatas.FirstOrDefault();
            resultTemplate = templateProfileResult.Render( new
            {
                rekapitulasiProduksi?.nm_cb, 
                kd_bln_mul = request.kd_bln_mul.ToString("dd-MM-yyyy"),
                kd_bln_akh = request.kd_bln_akh.ToString("dd-MM-yyyy"),
                details = stringBuilder.ToString()
            } );
            
            return resultTemplate;
        }
    }
}