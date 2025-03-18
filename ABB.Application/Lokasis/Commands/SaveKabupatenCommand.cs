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
    public class SaveKabupatenCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_prop { get; set; }

        public string kd_kab { get; set; }

        public string nm_kab { get; set; }
    }

    public class SaveKabupatenCommandHandler : IRequestHandler<SaveKabupatenCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveKabupatenCommandHandler> _logger;

        public SaveKabupatenCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveKabupatenCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveKabupatenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var kabupaten = dbContext.Kabupaten.FirstOrDefault(w => w.kd_prop == request.kd_prop
                                                                        && w.kd_kab == request.kd_kab);

                if (kabupaten == null)
                {
                    dbContext.Kabupaten.Add(new Kabupaten()
                    {
                        kd_prop = request.kd_prop,
                        kd_kab = request.kd_kab,
                        nm_kab = request.nm_kab
                    });
                }
                else
                    kabupaten.nm_kab = request.nm_kab;

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