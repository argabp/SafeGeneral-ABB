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
    }

    public class AddAsumsiCommandHandler : IRequestHandler<AddAsumsiCommand>
    {
        private readonly IDbContextCSM _dbContextCsm;
        private readonly ILogger<AddAsumsiCommandHandler> _logger;

        public AddAsumsiCommandHandler(IDbContextCSM dbContextCsm,
                                        ILogger<AddAsumsiCommandHandler> logger)
        {
            _dbContextCsm = dbContextCsm;
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

                _dbContextCsm.Asumsi.Add(asumsi);

                await _dbContextCsm.SaveChangesAsync(cancellationToken);
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