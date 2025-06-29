using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Asumsis.Commands
{
    public class DeleteAsumsiDetailCommand : IRequest
    {
        public string KodeAsumsi { get; set; }

        public string KodeProduk { get; set; }

        public DateTime PeriodeProses { get; set; }

        public Int16 Thn { get; set; }
        public string DatabaseName { get; set; }
    }

    public class DeleteAsumsiDetailCommandHandler : IRequestHandler<DeleteAsumsiDetailCommand>
    {
        private readonly IDbConnectionCSM _dbConnectionCsm;
        private readonly ILogger<DeleteAsumsiDetailCommandHandler> _logger;

        public DeleteAsumsiDetailCommandHandler(IDbConnectionCSM dbConnectionCsm, ILogger<DeleteAsumsiDetailCommandHandler> logger)
        {
            _dbConnectionCsm = dbConnectionCsm;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteAsumsiDetailCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                await _dbConnectionCsm.QueryProc("sp_DeleteAsumsiDetail",
                    new
                    {
                        request.KodeAsumsi, request.KodeProduk, request.PeriodeProses, request.Thn
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