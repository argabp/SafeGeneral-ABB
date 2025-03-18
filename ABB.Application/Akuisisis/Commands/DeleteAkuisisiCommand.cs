using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akuisisis.Commands
{
    public class DeleteAkuisisiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int kd_thn { get; set; }

        public string kd_mtu { get; set; }
    }

    public class DeleteAkuisisiCommandHandler : IRequestHandler<DeleteAkuisisiCommand>
    {
        private readonly ILogger<DeleteAkuisisiCommandHandler> _logger;
        private readonly IDbContextFactory _contextFactory;

        public DeleteAkuisisiCommandHandler(ILogger<DeleteAkuisisiCommandHandler> logger,
            IDbContextFactory contextFactory)
        {
            _logger = logger;
            _contextFactory = contextFactory;
        }

        public async Task<Unit> Handle(DeleteAkuisisiCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var akuisisi = dbContext.Akuisisi.FirstOrDefault(a => a.kd_mtu == request.kd_mtu
                                                                      && a.kd_scob == request.kd_scob
                                                                      && a.kd_cob == request.kd_cob
                                                                      && a.kd_thn == request.kd_thn);

                if (akuisisi != null)
                    dbContext.Akuisisi.Remove(akuisisi);

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