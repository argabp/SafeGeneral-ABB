using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.CetakSchedulePolis.Queries
{
    public class GetCetakSchedulePolisTemplateQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string no_aks { get; set; }
        public string? no_pol_ttg { get; set; }
        public int no_updt { get; set; }
        public string jenisLaporan { get; set; }
        public string bahasa { get; set; }
        public string jenisLampiran { get; set; }
    }

    public class GetCetakSchedulePolisTemplateQueryHandler : IRequestHandler<GetCetakSchedulePolisTemplateQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetCetakSchedulePolisTemplateQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GetCetakSchedulePolisTemplateQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var templateName = (await _connectionFactory.Query<string>("spi_uw01r_01", 
                new
                {
                    request.kd_cob, request.kd_scob, kd_lap = request.jenisLaporan,
                    kd_bhs = request.bahasa, kd_perhit_prm = request.jenisLampiran
                })).FirstOrDefault();

            if (templateName == null)
                throw new Exception("Template Not Found");

            return Constant.ReportMapping[templateName];
        }
    }
}