using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PengajuanAkseptasi.Queries
{
    public class GetPengajuanAkseptasiQuery : IRequest<TRAkseptasi>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }
    }

    public class GetPengajuanAkseptasiQueryHandler : IRequestHandler<GetPengajuanAkseptasiQuery, TRAkseptasi>
    {
        private readonly IDbConnection _db;
        private readonly ILogger<GetPengajuanAkseptasiQueryHandler> _logger;

        public GetPengajuanAkseptasiQueryHandler(IDbConnection db, ILogger<GetPengajuanAkseptasiQueryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<TRAkseptasi> Handle(GetPengajuanAkseptasiQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                return (await _db.Query<TRAkseptasi>(
                    @$"SELECT TOP 1 * FROM TR_Akseptasi WHERE kd_cb = '{request.kd_cb}' AND 
                               kd_cob = '{request.kd_cob}' AND kd_scob = '{request.kd_scob}' AND
                               kd_thn = '{request.kd_thn}' AND no_aks = '{request.no_aks}'")).FirstOrDefault();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return new TRAkseptasi();
        }
    }
}