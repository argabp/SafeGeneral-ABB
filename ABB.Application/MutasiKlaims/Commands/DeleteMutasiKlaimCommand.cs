using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.MutasiKlaims.Commands
{
    public class DeleteMutasiKlaimCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }
    }

    public class DeleteMutasiKlaimCommandHandler : IRequestHandler<DeleteMutasiKlaimCommand>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ILogger<DeleteMutasiKlaimCommandHandler> _logger;

        public DeleteMutasiKlaimCommandHandler(IDbContextFactory dbContextFactory, ILogger<DeleteMutasiKlaimCommandHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteMutasiKlaimCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                
                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);
                
                var mutasiKlaim = dbContext.MutasiKlaim.FirstOrDefault(w => w.kd_cb == request.kd_cb &&
                                                                              w.kd_cob == request.kd_cob &&
                                                                              w.kd_scob == request.kd_scob &&
                                                                              w.kd_thn == request.kd_thn &&
                                                                              w.no_kl == request.no_kl &&
                                                                              w.no_mts == request.no_mts);

                if (mutasiKlaim == null)
                {
                    throw new Exception("Data Mutasi Klaim Tidak Dapat Ditemukan");
                }
                
                var mutasiKlaimObyeks = dbContext.MutasiKlaimObyek.Where(w => w.kd_cb == request.kd_cb &&
                    w.kd_cob == request.kd_cob &&
                    w.kd_scob == request.kd_scob &&
                    w.kd_thn == request.kd_thn &&
                    w.no_kl == request.no_kl &&
                    w.no_mts == request.no_mts).ToList();
                
                var mutasiKlaimBebans = dbContext.MutasiKlaimBeban.Where(w => w.kd_cb == request.kd_cb &&
                                                                              w.kd_cob == request.kd_cob &&
                                                                              w.kd_scob == request.kd_scob &&
                                                                              w.kd_thn == request.kd_thn &&
                                                                              w.no_kl == request.no_kl &&
                                                                              w.no_mts == request.no_mts).ToList();
                
                var mutasiKlaimALokasis = dbContext.MutasiKlaimAlokasi.Where(w => w.kd_cb == request.kd_cb &&
                                                                              w.kd_cob == request.kd_cob &&
                                                                              w.kd_scob == request.kd_scob &&
                                                                              w.kd_thn == request.kd_thn &&
                                                                              w.no_kl == request.no_kl &&
                                                                              w.no_mts == request.no_mts).ToList();
                
                dbContext.MutasiKlaimObyek.RemoveRange(mutasiKlaimObyeks);
                dbContext.MutasiKlaimBeban.RemoveRange(mutasiKlaimBebans);
                dbContext.MutasiKlaimAlokasi.RemoveRange(mutasiKlaimALokasis);
                dbContext.MutasiKlaim.Remove(mutasiKlaim);

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