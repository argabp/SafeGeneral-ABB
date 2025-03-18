using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.GrupResikos.Commands
{
    public class EditDetailGrupResikoCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_grp_rsk { get; set; }

        public string kd_rsk { get; set; }

        public string desk_rsk { get; set; }

        public string kd_ref { get; set; }

        public string kd_ref1 { get; set; }
    }

    public class EditDetailGrupResikoCommandHandler : IRequestHandler<EditDetailGrupResikoCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<EditDetailGrupResikoCommandHandler> _logger;

        public EditDetailGrupResikoCommandHandler(IDbContextFactory contextFactory,
            ILogger<EditDetailGrupResikoCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditDetailGrupResikoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailGrupResiko = dbContext.DetailGrupResiko.FirstOrDefault(w => w.kd_grp_rsk == request.kd_grp_rsk
                                                                           && w.kd_rsk == request.kd_rsk);

                if (detailGrupResiko != null)
                {
                    detailGrupResiko.desk_rsk = request.desk_rsk;
                    detailGrupResiko.kd_ref = request.kd_ref;
                    detailGrupResiko.kd_ref1 = request.kd_ref1;
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