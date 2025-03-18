using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Okupasis.Commands
{
    public class SaveDetailOkupasiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_okup { get; set; }

        public string kd_kls_konstr { get; set; }

        public byte stn_rate_prm { get; set; }

        public decimal pst_rate_prm { get; set; }
    }

    public class SaveDetailOkupasiCommandHandler : IRequestHandler<SaveDetailOkupasiCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveDetailOkupasiCommandHandler> _logger;

        public SaveDetailOkupasiCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveDetailOkupasiCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveDetailOkupasiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailOkupasi = dbContext.DetailOkupasi.FirstOrDefault(w => w.kd_kls_konstr == request.kd_kls_konstr
                                                                                    && w.kd_okup == request.kd_okup);

                if (detailOkupasi == null)
                {
                    dbContext.DetailOkupasi.Add(new DetailOkupasi()
                    {
                        kd_kls_konstr = request.kd_kls_konstr,
                        kd_okup = request.kd_okup,
                        pst_rate_prm = request.pst_rate_prm,
                        stn_rate_prm = request.stn_rate_prm
                    });
                }
                else
                {
                    detailOkupasi.pst_rate_prm = request.pst_rate_prm;
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