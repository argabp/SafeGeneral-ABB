using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.GrupObyeks.Commands
{
    public class SaveGrupObyekCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_grp_oby { get; set; }

        public string nm_grp_oby { get; set; }
    }

    public class SaveGrupObyekCommandHandler : IRequestHandler<SaveGrupObyekCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveGrupObyekCommandHandler> _logger;

        public SaveGrupObyekCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveGrupObyekCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveGrupObyekCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var grupObyek = dbContext.GrupObyek.FirstOrDefault(w => w.kd_grp_oby == request.kd_grp_oby);

                if (grupObyek == null)
                {
                    dbContext.GrupObyek.Add(new GrupObyek()
                    {
                        kd_grp_oby = request.kd_grp_oby,
                        nm_grp_oby = request.nm_grp_oby
                    });
                }
                else
                    grupObyek.nm_grp_oby = request.nm_grp_oby;
                
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