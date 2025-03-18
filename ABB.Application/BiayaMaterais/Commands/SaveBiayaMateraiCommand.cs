using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.BiayaMaterais.Commands
{
    public class SaveBiayaMateraiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_mtu { get; set; }

        public decimal nilai_prm_mul { get; set; }
        
        public decimal nilai_prm_akh { get; set; }

        public decimal nilai_bia_mat { get; set; }
    }

    public class SaveBiayaMateraiCommandHandler : IRequestHandler<SaveBiayaMateraiCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveBiayaMateraiCommandHandler> _logger;

        public SaveBiayaMateraiCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveBiayaMateraiCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveBiayaMateraiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var biayaMaterai = dbContext.BiayaMaterai.FirstOrDefault(w => w.kd_mtu == request.kd_mtu
                                                                              && w.nilai_prm_akh == request.nilai_prm_akh
                                                                              && w.nilai_prm_mul == request.nilai_prm_mul);

                if (biayaMaterai == null)
                {
                    dbContext.BiayaMaterai.Add(new BiayaMaterai()
                    {
                        kd_mtu = request.kd_mtu,
                        nilai_bia_mat = request.nilai_bia_mat,
                        nilai_prm_akh = request.nilai_prm_akh,
                        nilai_prm_mul = request.nilai_prm_mul
                    });
                }
                else
                    biayaMaterai.nilai_bia_mat = request.nilai_bia_mat;
                
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                throw ex;
            }

            return Unit.Value;
        }
    }
}