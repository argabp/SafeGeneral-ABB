using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.MutasiKlaims.Commands
{
    public class DeleteMutasiKlaimAlokasiCommand : IRequest
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

    public class DeleteMutasiKlaimAlokasiCommandHandler : IRequestHandler<DeleteMutasiKlaimAlokasiCommand>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ILogger<DeleteMutasiKlaimAlokasiCommandHandler> _logger;

        public DeleteMutasiKlaimAlokasiCommandHandler(IDbContextFactory dbContextFactory, ILogger<DeleteMutasiKlaimAlokasiCommandHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteMutasiKlaimAlokasiCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);

                
                var mutasiKlaimAlokasi = await dbContext.MutasiKlaimAlokasi.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_kl, request.no_mts,
                    request.kd_grp_pas, request.kd_rk_pas);

                if (mutasiKlaimAlokasi == null)
                {
                    throw new Exception("Data Mutasi Klaim Alokasi Tidak Dapat Ditemukan");
                }

                dbContext.MutasiKlaimAlokasi.Remove(mutasiKlaimAlokasi);

                await dbContext.SaveChangesAsync(cancellationToken);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

            return Unit.Value;
        }
    }
}