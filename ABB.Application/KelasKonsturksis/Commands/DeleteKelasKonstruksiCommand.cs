using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KelasKonsturksis.Commands
{
    public class DeleteKelasKonstruksiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_kls_konstr { get; set; }
    }

    public class DeleteKelasKonstruksiCommandHandler : IRequestHandler<DeleteKelasKonstruksiCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteKelasKonstruksiCommandHandler> _logger;

        public DeleteKelasKonstruksiCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteKelasKonstruksiCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteKelasKonstruksiCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var kelasKonstruksi = dbContext.KelasKonstruksi.FirstOrDefault(kota => kota.kd_kls_konstr == request.kd_kls_konstr);

                if (kelasKonstruksi != null)
                    dbContext.KelasKonstruksi.Remove(kelasKonstruksi);

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