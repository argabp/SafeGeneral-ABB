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
        public string DatabaseName { get; set; }
    }

    public class DeleteAsumsiPeriodeCommandHandler : IRequestHandler<DeleteAsumsiPeriodeCommand>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<DeleteAsumsiPeriodeCommandHandler> _logger;

        public DeleteAsumsiPeriodeCommandHandler(IDbConnectionFactory dbConnectionFactory, ILogger<DeleteAsumsiPeriodeCommandHandler> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteAsumsiPeriodeCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                _dbConnectionFactory.CreateDbConnection(request.DatabaseName);
                await _dbConnectionFactory.QueryProc("sp_DeleteAsumsiPeriode",
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