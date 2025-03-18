using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.TertanggungPrincipals.Commands
{
    public class DeleteTertanggungPrincipalCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_grp_rk { get; set; }

        public string kd_rk { get; set; }
    }

    public class DeleteTertanggungPrincipalCommandHandler : IRequestHandler<DeleteTertanggungPrincipalCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<DeleteTertanggungPrincipalCommandHandler> _logger;

        public DeleteTertanggungPrincipalCommandHandler(IDbConnectionFactory connectionFactory, ILogger<DeleteTertanggungPrincipalCommandHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteTertanggungPrincipalCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                await _connectionFactory.QueryProc("sp_DeleteRekanan",
                    new
                    {
                        request.kd_cb, request.kd_grp_rk, request.kd_rk });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Unit.Value;
        }
    }
}