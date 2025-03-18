using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.GrupResikos.Commands
{
    public class AddGrupResikoCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_grp_rsk { get; set; }

        public string desk_grp_rsk { get; set; }

        public string kd_jns_grp { get; set; }
    }

    public class AddGrupResikoCommandHandler : IRequestHandler<AddGrupResikoCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<AddGrupResikoCommandHandler> _logger;

        public AddGrupResikoCommandHandler(IDbContextFactory contextFactory,
            ILogger<AddGrupResikoCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddGrupResikoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var grupResiko = new GrupResiko()
                {
                    desk_grp_rsk = request.desk_grp_rsk,
                    kd_jns_grp = request.kd_jns_grp,
                    kd_grp_rsk = request.kd_grp_rsk
                };

                dbContext.GrupResiko.Add(grupResiko);

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