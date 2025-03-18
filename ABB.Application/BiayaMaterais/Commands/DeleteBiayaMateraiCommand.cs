using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.BiayaMaterais.Commands
{
    public class DeleteBiayaMateraiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_mtu { get; set; }

        public decimal nilai_prm_mul { get; set; }
        
        public decimal nilai_prm_akh { get; set; }
    }

    public class DeleteBiayaMateraiCommandHandler : IRequestHandler<DeleteBiayaMateraiCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteBiayaMateraiCommandHandler> _logger;

        public DeleteBiayaMateraiCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteBiayaMateraiCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteBiayaMateraiCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var biayaMaterai = dbContext.BiayaMaterai.FirstOrDefault(biayaMaterai => biayaMaterai.kd_mtu == request.kd_mtu
                                                                            && biayaMaterai.nilai_prm_akh == request.nilai_prm_akh
                                                                            && biayaMaterai.nilai_prm_mul == request.nilai_prm_mul);

                if (biayaMaterai != null)
                    dbContext.BiayaMaterai.Remove(biayaMaterai);

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