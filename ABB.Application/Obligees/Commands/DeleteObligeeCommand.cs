using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Obligees.Commands
{
    public class DeleteObligeeCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_grp_rk { get; set; }

        public string kd_rk { get; set; }
    }

    public class DeleteObligeeCommandHandler : IRequestHandler<DeleteObligeeCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<DeleteObligeeCommandHandler> _logger;

        public DeleteObligeeCommandHandler(IDbConnectionFactory connectionFactory, ILogger<DeleteObligeeCommandHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteObligeeCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                await _connectionFactory.QueryProc("sp_DeleteObligee",
                    new
                    {
                        request.kd_cb, request.kd_grp_rk, request.kd_rk });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                throw ex.InnerException ?? ex;
            }

            return Unit.Value;
        }
    }
}