using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Lokasis.Commands
{
    public class DeleteLokasiResikoCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_pos { get; set; }
    }

    public class DeleteLokasiResikoCommandHandler : IRequestHandler<DeleteLokasiResikoCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<DeleteLokasiResikoCommandHandler> _logger;

        public DeleteLokasiResikoCommandHandler(IDbConnectionFactory connectionFactory, ILogger<DeleteLokasiResikoCommandHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteLokasiResikoCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                await _connectionFactory.QueryProc("sp_DeleteLokasiResiko",
                    new
                    {
                        request.kd_pos
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