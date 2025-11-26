using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.MutasiKlaims.Commands
{
    public class SaveMutasiKlaimCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public DateTime? tgl_closing { get; set; }
    }

    public class SaveMutasiKlaimCommandHandler : IRequestHandler<SaveMutasiKlaimCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveMutasiKlaimCommandHandler> _logger;

        public SaveMutasiKlaimCommandHandler(IDbContextFactory contextFactory, IDbConnectionFactory connectionFactory,
            ILogger<SaveMutasiKlaimCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveMutasiKlaimCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);

                var entity = await dbContext.MutasiKlaim.FindAsync(request.kd_cb,
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_kl, request.no_mts);

                if (entity == null)
                {
                    throw new Exception("Mutasi Klaim tidak dapat ditemukan");
                }
                
                _connectionFactory.CreateDbConnection(request.DatabaseName);

                entity.tgl_closing = request.tgl_closing;

                await dbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
                
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException == null ? e.Message : e.InnerException.Message);
                throw e.InnerException ?? e;
            }
        }
    }
}