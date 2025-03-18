using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Lokasis.Commands
{
    public class DeleteProvinsiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_prop { get; set; }
    }

    public class DeleteProvinsiCommandHandler : IRequestHandler<DeleteProvinsiCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<DeleteProvinsiCommandHandler> _logger;

        public DeleteProvinsiCommandHandler(IDbConnectionFactory connectionFactory, ILogger<DeleteProvinsiCommandHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteProvinsiCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                await _connectionFactory.QueryProc("sp_DeleteProvinsi",
                    new
                    {
                        request.kd_prop
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