using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.BiayaPerSubCOBs.Commands
{
    public class DeleteBiayaPerSubCOBCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_mtu { get; set; }
    }

    public class DeleteBiayaPerSubCOBCommandHandler : IRequestHandler<DeleteBiayaPerSubCOBCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteBiayaPerSubCOBCommandHandler> _logger;

        public DeleteBiayaPerSubCOBCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteBiayaPerSubCOBCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteBiayaPerSubCOBCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var biayaPerSubCOB = dbContext.BiayaPerSubCOB.FirstOrDefault(biayaPerSubCOB => biayaPerSubCOB.kd_mtu == request.kd_mtu
                    && biayaPerSubCOB.kd_scob == request.kd_scob
                    && biayaPerSubCOB.kd_cob == request.kd_cob);

                if (biayaPerSubCOB != null)
                    dbContext.BiayaPerSubCOB.Remove(biayaPerSubCOB);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Unit.Value;
        }
    }
}