using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.GrupObyeks.Commands
{
    public class DeleteGrupObyekCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_grp_oby { get; set; }
    }

    public class DeleteGrupObyekCommandHandler : IRequestHandler<DeleteGrupObyekCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteGrupObyekCommandHandler> _logger;

        public DeleteGrupObyekCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteGrupObyekCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteGrupObyekCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var grupObyek = dbContext.GrupObyek.FirstOrDefault(grupObyek => grupObyek.kd_grp_oby == request.kd_grp_oby);

                if (grupObyek != null)
                    dbContext.GrupObyek.Remove(grupObyek);

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