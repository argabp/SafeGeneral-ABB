using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.AkseptasiProduks.Commands
{
    public class AddAkseptasiProdukCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string Desc_AkseptasiProduk { get; set; }
    }

    public class AddAkseptasiProdukCommandHandler : IRequestHandler<AddAkseptasiProdukCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<AddAkseptasiProdukCommandHandler> _logger;

        public AddAkseptasiProdukCommandHandler(IDbContextFactory contextFactory,
            ILogger<AddAkseptasiProdukCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddAkseptasiProdukCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var akseptasiProduk = new AkseptasiProduk()
                {
                    kd_cob = request.kd_cob,
                    kd_scob = request.kd_scob,
                    Desc_AkseptasiProduk = request.Desc_AkseptasiProduk,
                };

                dbContext.AkseptasiProduk.Add(akseptasiProduk);

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