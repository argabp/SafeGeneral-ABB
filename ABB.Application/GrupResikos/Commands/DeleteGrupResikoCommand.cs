using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.GrupResikos.Commands
{
    public class DeleteGrupResikoCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_grp_rsk { get; set; }
    }

    public class DeleteGrupResikoCommandHandler : IRequestHandler<DeleteGrupResikoCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<DeleteGrupResikoCommandHandler> _logger;

        public DeleteGrupResikoCommandHandler(IDbConnectionFactory connectionFactory, ILogger<DeleteGrupResikoCommandHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteGrupResikoCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                await _connectionFactory.QueryProc("sp_DeleteGrupResiko",
                    new
                    {
                        request.kd_grp_rsk
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