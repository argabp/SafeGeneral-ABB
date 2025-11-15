using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.MutasiKlaims.Commands
{
    public class DeleteMutasiKlaimBebanCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public Int16 no_urut { get; set; }
    }

    public class DeleteMutasiKlaimBebanCommandHandler : IRequestHandler<DeleteMutasiKlaimBebanCommand>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ILogger<DeleteMutasiKlaimBebanCommandHandler> _logger;

        public DeleteMutasiKlaimBebanCommandHandler(IDbContextFactory dbContextFactory, ILogger<DeleteMutasiKlaimBebanCommandHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteMutasiKlaimBebanCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);

                var mutasiKlaimBeban = dbContext.MutasiKlaimBeban.FirstOrDefault(w => w.kd_cb == request.kd_cb &&
                    w.kd_cob == request.kd_cob &&
                    w.kd_scob == request.kd_scob &&
                    w.kd_thn == request.kd_thn &&
                    w.no_kl == request.no_kl &&
                    w.no_mts == request.no_mts &&
                    w.no_urut == request.no_urut);

                if (mutasiKlaimBeban == null)
                {
                    throw new Exception("Data Mutasi Klaim Obyek Tidak Dapat Ditemukan");
                }

                dbContext.MutasiKlaimBeban.Remove(mutasiKlaimBeban);

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