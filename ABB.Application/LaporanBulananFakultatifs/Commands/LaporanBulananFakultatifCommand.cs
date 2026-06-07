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

namespace ABB.Application.LaporanBulananFakultatifs.Commands
{
    public class LaporanBulananFakultatifCommand : IRequest<string>
    {
        public string kd_cob { get; set; }
        
        public DateTime tgl_mul { get; set; }
        
        public DateTime tgl_akh { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }

        public string jns_tr { get; set; }
    }

    public class LaporanBulananFakultatifCommandHandler : IRequestHandler<LaporanBulananFakultatifCommand, string>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly IHostEnvironment _environment;

        public LaporanBulananFakultatifCommandHandler(IDbConnectionPst connectionPst, IHostEnvironment environment)
        {
            _connectionPst = connectionPst;
            _environment = environment;
        }

        public async Task<string> Handle(LaporanBulananFakultatifCommand request, CancellationToken cancellationToken)
        {
            var datas = (await _connectionPst.QueryProc<LaporanBulananFakultatifModel>("spr_prod_fak", 
                new
                {
                    input_str = $"{request.kd_cob}, {request.tgl_mul}, {request.tgl_akh}, {request.kd_grp_pas}, {request.kd_rk_pas}, {request.jns_tr}"
                })).ToList();

            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "LaporanBulananFakultatif.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (datas.Count == 0)
            {
                throw new NullReferenceException("Data tidak ditemukan");
            }
            
            Template templateProfileResult = Template.Parse( templateReportHtml );

            string resultTemplate;

            StringBuilder stringBuilder = new StringBuilder();

            var header = request.jns_tr == "B2" ? "Fakultatif Masuk" : "Fakultatif Keluar";

            var sequence = 1;
            foreach (var data in datas)
            {
                var gross_premi = ReportHelper.ConvertToReportFormat(data.gross_premi);
                var komisi = ReportHelper.ConvertToReportFormat(data.komisi);
                var klaim = ReportHelper.ConvertToReportFormat(data.klaim);
                var netto = ReportHelper.ConvertToReportFormat(data.netto);

                var sequenceText = string.IsNullOrWhiteSpace(data.cob) ? string.Empty : sequence.ToString();
                stringBuilder.Append(@$"<tr>
                                            <td style='text-align: left; vertical-align: top; border: 1px solid'>{sequenceText}</td>
                                            <td style='text-align: left; vertical-align: top; border: 1px solid'>{data.cob}</td>
                                            <td style='text-align: left; vertical-align: top; border: 1px solid'>{data.ceding}</td>
                                            <td style='text-align: left; vertical-align: top; border: 1px solid'>{data.no_nota}</td>
                                            <td style='text-align: left; vertical-align: top; border: 1px solid'>{data.no_polis}</td>
                                            <td style='text-align: left; vertical-align: top; border: 1px solid'>{data.nm_ttg}</td>
                                            <td style='text-align: left; vertical-align: top; border: 1px solid'>{data.mtu}</td>
                                            <td style='text-align: right; vertical-align: top; border: 1px solid'>{gross_premi}</td>
                                            <td style='text-align: right; vertical-align: top; border: 1px solid'>{komisi}</td>
                                            <td style='text-align: right; vertical-align: top; border: 1px solid'>{klaim}</td>
                                            <td style='text-align: right; vertical-align: top; border: 1px solid'>{netto}</td>
                                        </tr>");
                sequence++;
            }
            
            resultTemplate = templateProfileResult.Render( new
            {
                data = stringBuilder.ToString(), header,
                tgl_mul = request.tgl_mul.ToString("dd MMMM yyyy"),
                tgl_akh = request.tgl_akh.ToString("dd MMMM yyyy")
            } );
            
            return resultTemplate;
        }
    }
}