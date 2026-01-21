using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.DashboardKlaims.Queries
{
    public class GetDashboardKlaimQuery : IRequest<DashboardKlaimDto>
    {
        public string KodeCabang { get; set; }

        public string Cabang { get; set; }

        public string DatabaseName { get; set; }
    }

    public class GetDashboardKlaimQueryHandler : IRequestHandler<GetDashboardKlaimQuery, DashboardKlaimDto>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public GetDashboardKlaimQueryHandler(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<DashboardKlaimDto> Handle(GetDashboardKlaimQuery request,
            CancellationToken cancellationToken)
        {
            var urls = new Dictionary<string, string>();
            
            urls.Add("New", "/RegisterKlaim/Index?status=New");
            urls.Add("LKS", "/RegisterKlaim/Index?status=LKS");
            urls.Add("LKP Process", "/RegisterKlaim/Index?status=LKP Process");
            urls.Add("LKP Revised", "/RegisterKlaim/Index?status=LKP Revised");
            urls.Add("LKP Escalated", "/RegisterKlaim/Index?status=LKP Escalated");
            urls.Add("LKP Settled", "/RegisterKlaim/Index?status=LKP Settled");
            urls.Add("Payment", "/RegisterKlaim/Index?status=Payment");
            // urls.Add("Rejected", "/RegisterKlaim/Index?");
            urls.Add("Closed", "/RegisterKlaim/Index?status=Closed");
            urls.Add("Final", "/RegisterKlaim/Index?status=Final");
            
            var icons = new Dictionary<string, string>();
            
            icons.Add("New", "fa-plus-circle");
            icons.Add("LKS", "fa-location-arrow");
            icons.Add("LKP Process", "fa-circle-notch");
            icons.Add("LKP Revised", "fa-edit");
            icons.Add("LKP Escalated", "fa-hand-paper");
            icons.Add("LKP Settled", "fa-child");
            icons.Add("Payment", "fa-credit-card");
            // icons.Add("Rejected", "fa-times-circle");
            icons.Add("Closed", "fa-stop-circle");
            icons.Add("Final", "fa-check-square");
            var dashboard = new DashboardKlaimDto();

            _dbConnectionFactory.CreateDbConnection(request.DatabaseName);
            
            var datas =
                (await _dbConnectionFactory.Query<DashboardKlaimDataDto>("SELECT * FROM v_tr_dashboard_klaim_cabang WHERE kd_cb = @KodeCabang", new { request.KodeCabang })).ToList();

            foreach (var url in urls)
            {
                if (datas.Any(w => w.nm_status == url.Key))
                {
                    dashboard.Data.Add(new DashboardKlaimDataDto()
                    {
                        nm_status = url.Key,
                        url = url.Value,
                        jml_data = datas.FirstOrDefault(w => w.nm_status == url.Key)?.jml_data,
                        icon = icons[url.Key]
                    });
                }
                else
                {
                    dashboard.Data.Add(new DashboardKlaimDataDto()
                    {
                        nm_status = url.Key,
                        url = url.Value,
                        jml_data = "0",
                        icon = icons[url.Key]
                    });
                }
            }
            
            var dateNow = DateTime.Today;
            var year = dateNow.Year;
            var month = dateNow.Month;
            var day = dateNow.Day;
            
            var graphic = (await _dbConnectionFactory.QueryProc<DashboardKlaimGraphicDto>("dbo.spr_ppc_bulanan07", new { kd_cb = request.KodeCabang, tgl_proses = $"{year}-{month}-{day}"  })).ToList();

            dashboard.Graphic = graphic;
            
            return dashboard;
        }
    }
}