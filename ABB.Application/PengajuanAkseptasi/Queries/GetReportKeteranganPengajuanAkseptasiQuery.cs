using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Scriban;

namespace ABB.Application.PengajuanAkseptasi.Queries
{
    public class GetReportKeteranganPengajuanAkseptasiQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_aks { get; set; }
    }

    public class GetReportKeteranganPengajuanAkseptasiQueryHandler : IRequestHandler<GetReportKeteranganPengajuanAkseptasiQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;

        public GetReportKeteranganPengajuanAkseptasiQueryHandler(IDbConnectionFactory connectionFactory, 
            IHostEnvironment environment, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            _connectionFactory = connectionFactory;
            _environment = environment;
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<string> Handle(GetReportKeteranganPengajuanAkseptasiQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            var datas = (await _connectionFactory.QueryProc<ReportKeteranganPengajuanAkseptasiDto>("sp_KeteranganStatusAks", 
                new
                {
                    input_str = $"{request.kd_cb.Trim()},{request.kd_cob.Trim()},{request.kd_scob.Trim()}," +
                                $"{request.kd_thn},{request.no_aks.Trim()}"
                })).ToList();
            
            string reportPath = Path.Combine( _environment.ContentRootPath, "Modules", "Reports", "Templates", "KeteranganPengajuanAkseptasi.html" );
            
            string templateReportHtml = await File.ReadAllTextAsync( reportPath );
            
            if (datas.Count == 0)
                throw new NullReferenceException("Data tidak ditemukan");

            Template templateProfileResult = Template.Parse( templateReportHtml );

            var wwwroot = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot");
            var path = _configuration.GetSection("UserSignature").Value.TrimEnd('/').TrimStart('/');
            
            var detail = new StringBuilder();
            var counter = 0;
            
            foreach (var data in datas)
            {
                counter++;
                
                var tgl_dibuat = data.tgl_status == null ? string.Empty : data.tgl_status.Value.ToString("dd MMM yyyy HH:mm:ss");

                detail.Append($@"<tr>
                    <td style='border: 1px solid black'>{counter}</td>
                    <td style='border: 1px solid black'>{data.nm_user}</td>
                    <td style='border: 1px solid black'>{tgl_dibuat}</td>
                    <td style='border: 1px solid black'>{data.nm_status}</td>
                    <td style='border: 1px solid black'>{data.ket_status}</td>
                </tr>");
            }
            
            var resultTemplate = templateProfileResult.Render( new
            {
                datas[0].nomor_pengajuan, detail = detail.ToString() 
            } );

            return resultTemplate;
        }
    }
}