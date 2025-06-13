using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Asumsis.Commands
{
    public class AddAsumsiCommand : IRequest
    {
        public string KodeAsumsi { get; set; }

        public string NamaAsumsi { get; set; }
        public string DatabaseName { get; set; }
    }

    public class AddAsumsiCommandHandler : IRequestHandler<AddAsumsiCommand>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ILogger<AddAsumsiCommandHandler> _logger;

        public AddAsumsiCommandHandler(IDbContextFactory dbContextFactory,
                                        ILogger<AddAsumsiCommandHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddAsumsiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var asumsi = new Domain.Entities.Asumsi()
                {
                    KodeAsumsi = request.KodeAsumsi,
                    NamaAsumsi = request.NamaAsumsi
                };

                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);
                dbContext.Asumsi.Add(asumsi);

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