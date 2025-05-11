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

        public string NamaCabang { get; set; }
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
            
            urls.Add("Submitted", "/PengaduanMasalah/SubmitView");
            urls.Add("Checked", "/PengaduanMasalah/ConfirmView");
            urls.Add("Escalated", "/PengaduanMasalah/WaitingView");
            urls.Add("Approved", "/PengaduanMasalah/ResponseView");
            urls.Add("Rejected", "/PengaduanMasalah/ProcessView");
            urls.Add("Revised", "/PengaduanMasalah/CheckinView");
            urls.Add("Canceled", "/PengaduanMasalah/ReopenView");
            
            var icons = new Dictionary<string, string>();
            
            icons.Add("Submitted", "fa-location-arrow");
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
            
            var graphic = (await _db.QueryProc<DashboardUnderwritingGraphicDto>("spr_ppc_bulanan_3", new { tgl_proses = $"{year}-{month}-{day}"  })).ToList();

            dashboard.Graphic = graphic.Where(w => w.nm_cab == request.NamaCabang).ToList();
            
            return dashboard;
        }
    }
}