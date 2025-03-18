using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.GrupResikos.Commands
{
    public class AddDetailGrupResikoCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_grp_rsk { get; set; }

        public string kd_rsk { get; set; }

        public string desk_rsk { get; set; }

        public string kd_ref { get; set; }

        public string kd_ref1 { get; set; }
    }

    public class AddDetailGrupResikoCommandHandler : IRequestHandler<AddDetailGrupResikoCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<AddDetailGrupResikoCommandHandler> _logger;

        public AddDetailGrupResikoCommandHandler(IDbContextFactory contextFactory,
            ILogger<AddDetailGrupResikoCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddDetailGrupResikoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailGrupResiko = new DetailGrupResiko()
                {
                    kd_grp_rsk = request.kd_grp_rsk,
                    desk_rsk = request.desk_rsk,
                    kd_ref = request.kd_ref,
                    kd_rsk = request.kd_rsk,
                    kd_ref1 = request.kd_ref1
                };

                dbContext.DetailGrupResiko.Add(detailGrupResiko);

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