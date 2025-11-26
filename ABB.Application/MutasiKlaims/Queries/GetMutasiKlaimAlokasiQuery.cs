using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.MutasiKlaims.Queries
{
    public class GetMutasiKlaimAlokasiQuery : IRequest<MutasiKlaimAlokasi>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public string kd_grp_pas { get; set; }
        
        public string kd_rk_pas { get; set; }
    }

    public class GetMutasiKlaimAlokasiQueryHandler : IRequestHandler<GetMutasiKlaimAlokasiQuery, MutasiKlaimAlokasi>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ILogger<GetMutasiKlaimAlokasiQueryHandler> _logger;

        public GetMutasiKlaimAlokasiQueryHandler(IDbContextFactory dbContextFactory, ILogger<GetMutasiKlaimAlokasiQueryHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<MutasiKlaimAlokasi> Handle(GetMutasiKlaimAlokasiQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);

                
                var mutasiKlaimAlokasi = await dbContext.MutasiKlaimAlokasi.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_kl, request.no_mts,
                    request.kd_grp_pas, request.kd_rk_pas);

                return mutasiKlaimAlokasi;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}