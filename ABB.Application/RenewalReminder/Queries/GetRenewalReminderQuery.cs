using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.RekapitulasiProduksi.Quries;
using MediatR;
using Microsoft.Extensions.Hosting;
using Scriban;

namespace ABB.Application.RenewalReminder.Queries
{
    public class GetRenewalReminderQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public DateTime kd_bln_mul { get; set; }
        public DateTime kd_bln_akh { get; set; }
        
        public string kd_cob { get; set; }
    }

    public class GetRenewalReminderQueryHandler : IRequestHandler<GetRenewalReminderQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;

        public GetRenewalReminderQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
        }

        public async Task<string> Handle(GetRenewalReminderQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            var renewalReminderDatas = (await _connectionFactory.QueryProc<RenewalReminderDto>("spr_mkt02r_03", 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()},{request.kd_bln_mul:yyyy/MM/dd},{request.kd_bln_akh:yyyy/MM/dd},"
                })).ToList();

            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "RenewalReminder.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (renewalReminderDatas.Count == 0)
            {
                throw new NullReferenceException("Data tidak ditemukan");
            }
            
            Template templateProfileResult = Template.Parse( templateReportHtml );

            string resultTemplate;

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($@"<table class='table'>
                                    <tr>
                                        <td style='width: 3%; text-align: center; border: 1px solid' rowspan=2>No.</td>
                                        <td style='width: 20%; text-align: center; border: 1px solid' rowspan=2>Nomor Polis</td>
                                        <td style='width: 20%; text-align: center; border: 1px solid' rowspan=2>Nama Tertanggung</td>
                                        <td style='width: 20%; text-align: center; border: 1px solid' rowspan=2>Harga Pertanggungan</td>
                                        <td style='width: 10%; text-align: center; border: 1px solid' rowspan=2>Premi</td>
                                        <td style='width: 5%;  text-align: center; border: 1px solid' colspan=3>Jatuh Tempo</td>
                                    </tr>
                                    <tr>
                                        <td style='border: 1px solid; text-align: center; width: 2%'>Tanggal</td>
                                        <td style='border: 1px solid; text-align: center; width: 2%'>Bulan</td>
                                        <td style='border: 1px solid; text-align: center; width: 2%'>Tahun</td>
                                    </tr>");

            var groups = renewalReminderDatas.Select(s => s.nm_cob?.Trim() + "|" + s.nm_scob?.Trim()).Distinct().ToList();

            foreach (var group in groups)
            {
                int sequence = 0;
                var groupData = group.Split("|");
                stringBuilder.Append($@"
                                        <tr>
                                            <td colspan='10' style='border-bottom: 1px solid;border-top: 1px solid;'>{groupData[0]} - {groupData[1]}</td>
                                        </tr>");
                
                foreach (var data in renewalReminderDatas.Where(w => w.nm_cob?.Trim() + "|" + w.nm_scob?.Trim() == group))
                {
                    sequence++;
                    var nilai_prm = ReportHelper.ConvertToReportFormat(data.nilai_prm);
                    var nilai_ptg = ReportHelper.ConvertToReportFormat(data.nilai_ptg);
                    
                    stringBuilder.Append(@$"
                                        <tr>
                                            <td style='width: 3%;  text-align: left; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{sequence}</td>
                                            <td style='width: 20%; text-align: left; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{data.no_pol_ttg}</td>
                                            <td style='width: 20%; text-align: left; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{data.nm_ttg}</td>
                                            <td style='width: 20%; text-align: right; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{nilai_ptg}</td>
                                            <td style='width: 20%; text-align: right; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{nilai_prm}</td>
                                            <td style='width: 5%;  text-align: center; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{data.tgl_akh_ptg_ind:dd}</td>
                                            <td style='width: 10%; text-align: center; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{data.tgl_akh_ptg_ind:MMM}</td>
                                            <td style='width: 10%; text-align: center; vertical-align: top; border-left: 1px solid; border-right: 1px solid;'>{data.tgl_akh_ptg_ind:yyyy}</td>
                                        </tr>");
                }
            }
            
            stringBuilder.Append("<tr><td style='border-top:1px solid' colspan=8></td></tr></table>");
            
            var renewalReminder = renewalReminderDatas.FirstOrDefault();
            resultTemplate = templateProfileResult.Render( new
            {
                renewalReminder?.nm_cb, 
                kd_bln_mul = request.kd_bln_mul.ToString("dd-MM-yyyy"),
                kd_bln_akh = request.kd_bln_akh.ToString("dd-MM-yyyy"),
                details = stringBuilder.ToString()
            } );
            
            return resultTemplate;
        }
    }
}