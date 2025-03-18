using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.MataUangs.Commands
{
    public class AddDetailMataUangCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_mtu { get; set; }

        public DateTime tgl_mul { get; set; }
        
        public DateTime tgl_akh { get; set; }

        public decimal nilai_kurs { get; set; }
    }

    public class AddDetailMataUangCommandHandler : IRequestHandler<AddDetailMataUangCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<AddDetailMataUangCommandHandler> _logger;

        public AddDetailMataUangCommandHandler(IDbContextFactory contextFactory,
            ILogger<AddDetailMataUangCommandHandler> logger)
        {;
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddDetailMataUangCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailMataUang = new DetailMataUang()
                {
                    kd_mtu = request.kd_mtu,
                    nilai_kurs = request.nilai_kurs,
                    tgl_akh = request.tgl_akh,
                    tgl_mul = request.tgl_mul
                };

                dbContext.DetailMataUang.Add(detailMataUang);

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