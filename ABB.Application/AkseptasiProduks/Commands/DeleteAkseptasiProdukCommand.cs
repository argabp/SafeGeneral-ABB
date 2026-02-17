using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.AkseptasiProduks.Commands
{
    public class DeleteAkseptasiProdukCommand : IRequest
    {
        public string DatabaseName { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }
    }

    public class DeleteAkseptasiProdukCommandHandler : IRequestHandler<DeleteAkseptasiProdukCommand>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ILogger<DeleteAkseptasiProdukCommandHandler> _logger;

        public DeleteAkseptasiProdukCommandHandler(IDbContextFactory dbContextFactory, ILogger<DeleteAkseptasiProdukCommandHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteAkseptasiProdukCommand request,
            CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
             {
                 var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);

                 var akseptasiProduk = dbContext.AkseptasiProduk.FirstOrDefault(w =>
                     w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob);

                 if (akseptasiProduk != null)
                 {
                     dbContext.AkseptasiProduk.Remove(akseptasiProduk);

                     await dbContext.SaveChangesAsync(cancellationToken);
                 }
             }, _logger);

            return Unit.Value;
        }
    }
}