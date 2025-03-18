using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Lokasis.Commands
{
    public class SaveProvinsiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_prop { get; set; }

        public string nm_prop { get; set; }

        public string no_pos { get; set; }

        public string kd_wilayah { get; set; }
    }

    public class SaveProvinsiCommandHandler : IRequestHandler<SaveProvinsiCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveProvinsiCommandHandler> _logger;

        public SaveProvinsiCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveProvinsiCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveProvinsiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var provinsi = dbContext.Provinsi.FirstOrDefault(w => w.kd_prop == request.kd_prop);

                if (provinsi == null)
                {
                    dbContext.Provinsi.Add(new Provinsi()
                    {
                        kd_prop = request.kd_prop,
                        kd_wilayah = request.kd_wilayah,
                        nm_prop = request.nm_prop,
                        no_pos = request.no_pos
                    });
                }
                else
                {
                    provinsi.kd_wilayah = request.kd_wilayah;
                    provinsi.nm_prop = request.nm_prop;
                    provinsi.no_pos = request.no_pos;                    
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