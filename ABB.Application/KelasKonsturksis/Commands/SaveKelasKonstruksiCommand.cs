using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KelasKonsturksis.Commands
{
    public class SaveKelasKonstruksiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_kls_konstr { get; set; }

        public string nm_kls_konstr { get; set; }
    }

    public class SaveKelasKonstruksiCommandHandler : IRequestHandler<SaveKelasKonstruksiCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveKelasKonstruksiCommandHandler> _logger;

        public SaveKelasKonstruksiCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveKelasKonstruksiCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveKelasKonstruksiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var kelasKonstruksi = dbContext.KelasKonstruksi.FirstOrDefault(w => w.kd_kls_konstr == request.kd_kls_konstr);

                if (kelasKonstruksi == null)
                {
                    dbContext.KelasKonstruksi.Add(new KelasKonstruksi()
                    {
                        kd_kls_konstr = request.kd_kls_konstr,
                        nm_kls_konstr = request.nm_kls_konstr
                    });
                }
                else
                    kelasKonstruksi.nm_kls_konstr = request.nm_kls_konstr;
                
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