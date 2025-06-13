using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Asumsis.Commands
{
    public class EditAsumsiCommand : IRequest
    {
        public string KodeAsumsi { get; set; }

        public string NamaAsumsi { get; set; }
        public string DatabaseName { get; set; }
    }

    public class EditAsumsiCommandHandler : IRequestHandler<EditAsumsiCommand>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ILogger<EditAsumsiCommandHandler> _logger;

        public EditAsumsiCommandHandler(IDbContextFactory dbContextFactory,
            ILogger<EditAsumsiCommandHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditAsumsiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);
                var asumsi = dbContext.Asumsi.FirstOrDefault(w => w.KodeAsumsi == request.KodeAsumsi);

                if (asumsi != null)
                    asumsi.NamaAsumsi = request.NamaAsumsi;

                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                throw ex;
            }

            return Unit.Value;
        }
    }
}