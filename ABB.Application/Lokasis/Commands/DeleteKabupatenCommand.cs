using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Lokasis.Commands
{
    public class DeleteKabupatenCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_prop { get; set; }

        public string kd_kab { get; set; }
    }

    public class DeleteKabupatenCommandHandler : IRequestHandler<DeleteKabupatenCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<DeleteKabupatenCommandHandler> _logger;

        public DeleteKabupatenCommandHandler(IDbConnectionFactory connectionFactory, ILogger<DeleteKabupatenCommandHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteKabupatenCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                await _connectionFactory.QueryProc("sp_DeleteKabupaten",
                    new
                    {
                        request.kd_prop,
                        request.kd_kab
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