using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Kapals.Commands
{
    public class DeleteKapalCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_kapal { get; set; }
    }

    public class DeleteKapalCommandHandler : IRequestHandler<DeleteKapalCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteKapalCommandHandler> _logger;

        public DeleteKapalCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteKapalCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteKapalCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var kapal = dbContext.Kapal.FirstOrDefault(kapal => kapal.kd_kapal == request.kd_kapal);

                if (kapal != null)
                    dbContext.Kapal.Remove(kapal);

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