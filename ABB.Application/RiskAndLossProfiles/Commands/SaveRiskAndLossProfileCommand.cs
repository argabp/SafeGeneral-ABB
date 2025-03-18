using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.RiskAndLossProfiles.Commands
{
    public class SaveRiskAndLossProfileCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public int nomor { get; set; }

        public decimal bts1 { get; set; }
        
        public decimal bts2 { get; set; }
    }

    public class SaveRiskAndLossProfileCommandHandler : IRequestHandler<SaveRiskAndLossProfileCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveRiskAndLossProfileCommandHandler> _logger;

        public SaveRiskAndLossProfileCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveRiskAndLossProfileCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveRiskAndLossProfileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var riskAndLossProfile = dbContext.RiskAndLossProfile.FirstOrDefault(w => w.kd_cob == request.kd_cob
                                                                        && w.nomor == request.nomor);

                if (riskAndLossProfile == null)
                {
                    dbContext.RiskAndLossProfile.Add(new RiskAndLossProfile()
                    {
                        kd_cob = request.kd_cob,
                        nomor = request.nomor,
                        bts1 = request.bts1,
                        bts2 = request.bts2
                    });
                }
                else
                {
                    riskAndLossProfile.bts1 = request.bts1;    
                    riskAndLossProfile.bts2 = request.bts2;    
                }

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