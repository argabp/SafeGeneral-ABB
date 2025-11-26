using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.MutasiKlaims.Queries
{
    public class GetMutasiKlaimObyekQuery : IRequest<MutasiKlaimObyek>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public Int16 no_oby { get; set; }
    }

    public class GetMutasiKlaimObyekQueryHandler : IRequestHandler<GetMutasiKlaimObyekQuery, MutasiKlaimObyek>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ILogger<GetMutasiKlaimObyekQueryHandler> _logger;

        public GetMutasiKlaimObyekQueryHandler(IDbContextFactory dbContextFactory, ILogger<GetMutasiKlaimObyekQueryHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<MutasiKlaimObyek> Handle(GetMutasiKlaimObyekQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);
                
                var mutasiKlaimObyek = dbContext.MutasiKlaimObyek.FirstOrDefault(w => w.kd_cb == request.kd_cb &&
                    w.kd_cob == request.kd_cob &&
                    w.kd_scob == request.kd_scob &&
                    w.kd_thn == request.kd_thn &&
                    w.no_kl == request.no_kl &&
                    w.no_mts == request.no_mts &&
                    w.no_oby == request.no_oby);

                return mutasiKlaimObyek;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}