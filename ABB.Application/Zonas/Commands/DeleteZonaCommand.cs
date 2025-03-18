using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Zonas.Commands
{
    public class DeleteZonaCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_zona { get; set; }
    }

    public class DeleteZonaCommandHandler : IRequestHandler<DeleteZonaCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<DeleteZonaCommandHandler> _logger;

        public DeleteZonaCommandHandler(IDbConnectionFactory connectionFactory, ILogger<DeleteZonaCommandHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteZonaCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                await _connectionFactory.QueryProc("sp_DeleteZona",
                    new
                    {
                        request.kd_zona
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