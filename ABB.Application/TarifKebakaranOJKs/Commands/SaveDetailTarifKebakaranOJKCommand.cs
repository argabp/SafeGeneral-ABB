using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.TarifKebakaranOJKs.Commands
{
    public class SaveDetailTarifKebakaranOJKCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_okup { get; set; }

        public string kd_kls_konstr { get; set; }

        public byte stn_rate_prm { get; set; }

        public decimal pst_rate_prm_min { get; set; }
        
        public decimal pst_rate_prm_max { get; set; }
    }

    public class SaveDetailTarifKebakaranOJKCommandHandler : IRequestHandler<SaveDetailTarifKebakaranOJKCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveDetailTarifKebakaranOJKCommandHandler> _logger;

        public SaveDetailTarifKebakaranOJKCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveDetailTarifKebakaranOJKCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveDetailTarifKebakaranOJKCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailOkupasi = dbContext.DetailTarifKebakaranOJK.FirstOrDefault(w => w.kd_kls_konstr == request.kd_kls_konstr
                                                                                    && w.kd_okup == request.kd_okup);

                if (detailOkupasi == null)
                {
                    dbContext.DetailTarifKebakaranOJK.Add(new DetailTarifKebakaranOJK()
                    {
                        kd_kls_konstr = request.kd_kls_konstr,
                        kd_okup = request.kd_okup,
                        pst_rate_prm_min = request.pst_rate_prm_min,
                        pst_rate_prm_max = request.pst_rate_prm_max,
                        stn_rate_prm = request.stn_rate_prm
                    });
                }
                else
                {
                    detailOkupasi.pst_rate_prm_min = request.pst_rate_prm_min;
                    detailOkupasi.pst_rate_prm_max = request.pst_rate_prm_max;
                    detailOkupasi.stn_rate_prm = request.stn_rate_prm;
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