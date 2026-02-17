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
    public class EditAkseptasiProdukCommand : IRequest
    {
        public string DatabaseName { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string Desc_AkseptasiProduk { get; set; }
    }

    public class EditAkseptasiProdukCommandHandler : IRequestHandler<EditAkseptasiProdukCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<EditAkseptasiProdukCommandHandler> _logger;

        public EditAkseptasiProdukCommandHandler(IDbContextFactory contextFactory,
            ILogger<EditAkseptasiProdukCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditAkseptasiProdukCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
             {
                 var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                 var akseptasiProduk = dbContext.AkseptasiProduk.FirstOrDefault(w =>
                     w.kd_cob == request.kd_cob && w.kd_scob == request.kd_scob);

                 if (akseptasiProduk != null)
                 {
                     akseptasiProduk.Desc_AkseptasiProduk = request.Desc_AkseptasiProduk;
                    
                     await dbContext.SaveChangesAsync(cancellationToken);
                 }
             }, _logger);

            return Unit.Value;
        }
    }
}