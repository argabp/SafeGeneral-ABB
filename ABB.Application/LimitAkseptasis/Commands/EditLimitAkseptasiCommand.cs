using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LimitAkseptasis.Commands
{
    public class EditLimitAkseptasiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }

        public string nm_limit { get; set; }

        public decimal? pst_limit_cb { get; set; }

        public Int16? maks_panel { get; set; }
        public decimal? nilai_kapasitas { get; set; }
    }

    public class EditLimitAkseptasiCommandHandler : IRequestHandler<EditLimitAkseptasiCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<EditLimitAkseptasiCommandHandler> _logger;

        public EditLimitAkseptasiCommandHandler(IDbContextFactory contextFactory,
            ILogger<EditLimitAkseptasiCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditLimitAkseptasiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var limitAkseptasi = dbContext.LimitAkseptasi.FirstOrDefault(w =>
                    w.kd_cb == request.kd_cb && w.kd_cob == request.kd_cob 
                                             && w.kd_scob == request.kd_scob
                                             && w.thn == request.thn);

                if (limitAkseptasi != null)
                {
                    limitAkseptasi.nm_limit = request.nm_limit;
                    limitAkseptasi.pst_limit_cb = request.pst_limit_cb;
                    limitAkseptasi.maks_panel = request.maks_panel;
                    limitAkseptasi.nilai_kapasitas = request.nilai_kapasitas;
                    
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

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