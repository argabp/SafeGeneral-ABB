using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Asumsis.Commands
{
    public class DeleteAsumsiCommand : IRequest
    {
        public string KodeAsumsi { get; set; }
        public string DatabaseName { get; set; }
    }

    public class DeleteAsumsiCommandHandler : IRequestHandler<DeleteAsumsiCommand>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<DeleteAsumsiCommandHandler> _logger;

        public DeleteAsumsiCommandHandler(IDbConnectionFactory dbConnectionFactory, ILogger<DeleteAsumsiCommandHandler> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteAsumsiCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                _dbConnectionFactory.CreateDbConnection(request.DatabaseName);
                await _dbConnectionFactory.QueryProc("sp_DeleteAsumsi",
                    new
                    {
                        request.KodeAsumsi
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Unit.Value;
        }
    }
}