using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PertanggunganKendaraans.Commands
{
    public class DeletePertanggunganKendaraanCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_jns_ptg { get; set; }
    }

    public class DeletePertanggunganKendaraanCommandHandler : IRequestHandler<DeletePertanggunganKendaraanCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<DeletePertanggunganKendaraanCommandHandler> _logger;

        public DeletePertanggunganKendaraanCommandHandler(IDbConnectionFactory connectionFactory, ILogger<DeletePertanggunganKendaraanCommandHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeletePertanggunganKendaraanCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                await _connectionFactory.QueryProc("sp_DeletePertanggunganKendaraan",
                    new
                    {
                        request.kd_cob, request.kd_scob, request.kd_jns_ptg
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