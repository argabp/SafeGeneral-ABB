using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.GrupResikos.Commands
{
    public class EditGrupResikoCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_grp_rsk { get; set; }

        public string desk_grp_rsk { get; set; }

        public string kd_jns_grp { get; set; }
    }

    public class EditGrupResikoCommandHandler : IRequestHandler<EditGrupResikoCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<EditGrupResikoCommandHandler> _logger;

        public EditGrupResikoCommandHandler(IDbContextFactory contextFactory,
            ILogger<EditGrupResikoCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditGrupResikoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var grupResiko = dbContext.GrupResiko.FirstOrDefault(w => w.kd_grp_rsk == request.kd_grp_rsk);

                if (grupResiko != null)
                {
                    grupResiko.desk_grp_rsk = request.desk_grp_rsk;
                    grupResiko.kd_jns_grp = request.kd_jns_grp;
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