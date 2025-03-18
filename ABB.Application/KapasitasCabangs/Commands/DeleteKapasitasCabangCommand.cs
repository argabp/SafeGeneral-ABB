using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KapasitasCabangs.Commands
{
    public class DeleteKapasitasCabangCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }
    }

    public class DeleteKapasitasCabangCommandHandler : IRequestHandler<DeleteKapasitasCabangCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteKapasitasCabangCommandHandler> _logger;

        public DeleteKapasitasCabangCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteKapasitasCabangCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteKapasitasCabangCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var kapasitasCabang = dbContext.KapasitasCabang.FirstOrDefault(kapasitasCabang => kapasitasCabang.kd_cb == request.kd_cb
                                                                              && kapasitasCabang.kd_scob == request.kd_scob
                                                                              && kapasitasCabang.kd_cob == request.kd_cob
                                                                              && kapasitasCabang.thn == request.thn);

                if (kapasitasCabang != null)
                    dbContext.KapasitasCabang.Remove(kapasitasCabang);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Unit.Value;
        }
    }
}