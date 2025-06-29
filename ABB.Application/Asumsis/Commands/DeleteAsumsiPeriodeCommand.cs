using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Asumsis.Commands
{
    public class DeleteAsumsiPeriodeCommand : IRequest
    {
        public string KodeAsumsi { get; set; }

        public string KodeProduk { get; set; }

        public DateTime PeriodeProses { get; set; }
    }

    public class DeleteAsumsiPeriodeCommandHandler : IRequestHandler<DeleteAsumsiPeriodeCommand>
    {
        private readonly IDbConnectionCSM _dbConnectionCsm;
        private readonly ILogger<DeleteAsumsiPeriodeCommandHandler> _logger;

        public DeleteAsumsiPeriodeCommandHandler(IDbConnectionCSM dbConnectionCsm, ILogger<DeleteAsumsiPeriodeCommandHandler> logger)
        {
            _dbConnectionCsm = dbConnectionCsm;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteAsumsiPeriodeCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                await _dbConnectionCsm.QueryProc("sp_DeleteAsumsiPeriode",
                    new
                    {
                        request.KodeAsumsi, request.KodeProduk, request.PeriodeProses
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