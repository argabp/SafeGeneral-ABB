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

namespace ABB.Application.OutstandingKlaim.Queries
{
    public class GetOutstandingKlaimQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }
        
        public DateTime kd_mul { get; set; }
        
        public DateTime kd_akh { get; set; }

        public string jenis_laporan { get; set; }
    }

    public class GetOutstandingKlaimQueryHandler : IRequestHandler<GetOutstandingKlaimQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;

        public GetOutstandingKlaimQueryHandler(IDbConnectionFactory connectionFactory, IHostEnvironment environment)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
        }

        public async Task<string> Handle(GetOutstandingKlaimQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            var sp_name = string.Empty;

            switch (request.jenis_laporan)
            {
                case "P":
                    sp_name = "spr_cl06r_01_bgu";
                    break;
                case "R":
                    sp_name = "spr_cl06r_02";
                    break;
            }
            
            var outstandingKlaimDatas = (await _connectionFactory.QueryProc<OutstandingKlaimDto>(sp_name, 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()}," +
                                $"{request.kd_mul.ToShortDateString()},{request.kd_akh.ToShortDateString()}"
                })).ToList();

            var report_name = string.Empty;

            switch (request.jenis_laporan)
            {
                case "P":
                    report_name = "OutstandingKlaimRincian.html";
                    break;
                case "R":
                    report_name = "OutstandingKlaimRekap.html";
                    break;
            }
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", report_name );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (outstandingKlaimDatas.Count == 0)
            {
                throw new NullReferenceException("Data tidak ditemukan");
            }
            
            Template templateProfileResult = Template.Parse( templateReportHtml );

            string resultTemplate;

            var outstandingKlaimData = outstandingKlaimDatas.FirstOrDefault();

            StringBuilder stringBuilder = new StringBuilder();
            var sequence = 0;
            foreach (var data in outstandingKlaimDatas)
            {
                sequence++;
                
                switch (request.jenis_laporan)
                {
                    case "P":
                        
                        break;
                    case "R":
                        
                        break;
                }
                
                stringBuilder.Append(@$"<tr>
                                            <td style='width: 3%;  text-align: left; vertical-align: top; border: 1px solid'>{sequence}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{0}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{0}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{0}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{0}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{0}</td>
                                            <td style='width: 10%; text-align: right; vertical-align: top; border: 1px solid'>{0}</td>
                                        </tr>");
            }
            
            resultTemplate = templateProfileResult.Render( new
            {
                details = stringBuilder.ToString(),
                outstandingKlaimData.no_pol_ttg, outstandingKlaimData.nm_cb,
                outstandingKlaimData.nm_cob, tgl_akh = outstandingKlaimData.tgl_akh.Value.ToString("dd MMMM yyyy")
            } );
            
            return resultTemplate;
        }
    }
}