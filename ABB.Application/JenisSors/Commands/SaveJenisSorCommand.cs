using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.JenisSors.Commands
{
    public class SaveJenisSorCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_jns_sor { get; set; }

        public string nm_jns_sor { get; set; }

        public string grp_jns_sor { get; set; }

        public string? no_urut { get; set; }
    }

    public class SaveJenisSorCommandHandler : IRequestHandler<SaveJenisSorCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveJenisSorCommandHandler> _logger;

        public SaveJenisSorCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveJenisSorCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveJenisSorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var jenisSor = dbContext.JenisSor.FirstOrDefault(jenisSor => jenisSor.kd_jns_sor == request.kd_jns_sor);

                if (jenisSor == null)
                {
                    dbContext.JenisSor.Add(new JenisSor()
                    {
                        kd_jns_sor = request.kd_jns_sor,
                        nm_jns_sor = request.nm_jns_sor,
                        grp_jns_sor = Int16.Parse(request.grp_jns_sor),
                        no_urut = null
                    });
                }
                else {
                    jenisSor.nm_jns_sor = request.nm_jns_sor;
                    jenisSor.grp_jns_sor = Int16.Parse(request.grp_jns_sor);
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