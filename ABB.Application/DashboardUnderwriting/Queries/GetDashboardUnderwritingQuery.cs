using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.DashboardUnderwriting.Queries
{
    public class GetDashboardUnderwritingQuery : IRequest<DashboardUnderwritingDto>
    {
        public string KodeCabang { get; set; }

        public string Cabang { get; set; }
    }

    public class GetDashboardUnderwritingQueryHandler : IRequestHandler<GetDashboardUnderwritingQuery, DashboardUnderwritingDto>
    {
        private readonly IDbConnection _db;

        public GetDashboardUnderwritingQueryHandler(IDbConnection db)
        {
            _db = db;
        }

        public async Task<DashboardUnderwritingDto> Handle(GetDashboardUnderwritingQuery request,
            CancellationToken cancellationToken)
        {
            var urls = new Dictionary<string, string>();
            
            urls.Add("New", "/PengajuanAkseptasi/Index?");
            urls.Add("Submited", "/PengajuanAkseptasi/Index?");
            urls.Add("Checked", "/PengajuanAkseptasi/Index?");
            urls.Add("Escalated", "/PengajuanAkseptasi/Index?");
            urls.Add("Approved", "/PengajuanAkseptasi/Index?");
            urls.Add("Rejected", "/PengajuanAkseptasi/Index?");
            urls.Add("Revised", "/PengajuanAkseptasi/Index?");
            urls.Add("Canceled", "/PengajuanAkseptasi/Index?");
            
            var icons = new Dictionary<string, string>();
            
            icons.Add("New", "fa-plus-circle");
            icons.Add("Submited", "fa-location-arrow");
            icons.Add("Checked", "fa-check-square");
            icons.Add("Escalated", "fa-hand-paper");
            icons.Add("Approved", "fa-child");
            icons.Add("Rejected", "fa-times-circle");
            icons.Add("Revised", "fa-edit");
            icons.Add("Canceled", "fa-ban");
            var dashboard = new DashboardUnderwritingDto();

            var datas =
                (await _db.Query<DashboardUnderwritingDataDto>("SELECT * FROM v_TR_Dashboard_Cabang WHERE kd_cb = @KodeCabang", new { request.KodeCabang })).ToList();

            foreach (var url in urls)
            {
                if (datas.Any(w => w.nm_status == url.Key))
                {
                    dashboard.Data.Add(new DashboardUnderwritingDataDto()
                    {
                        nm_status = url.Key,
                        url = url.Value,
                        jml_data = datas.FirstOrDefault(w => w.nm_status == url.Key)?.jml_data,
                        icon = icons[url.Key]
                    });
                }
                else
                {
                    dashboard.Data.Add(new DashboardUnderwritingDataDto()
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
            
            var graphic = (await _db.QueryProc<DashboardUnderwritingGraphicDto>("spr_ppc_bulanan_1", new { kd_cb = request.KodeCabang, tgl_proses = $"{year}-{month}-{day}"  })).ToList();

            dashboard.Graphic = graphic;
            
            return dashboard;
        }
    }
}