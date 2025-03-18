using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Okupasis.Commands
{
    public class DeleteOkupasiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_okup { get; set; }
    }

    public class DeleteOkupasiCommandHandler : IRequestHandler<DeleteOkupasiCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<DeleteOkupasiCommandHandler> _logger;

        public DeleteOkupasiCommandHandler(IDbConnectionFactory connectionFactory, ILogger<DeleteOkupasiCommandHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteOkupasiCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                await _connectionFactory.QueryProc("sp_DeleteOkupasi",
                    new
                    {
                        request.kd_okup
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