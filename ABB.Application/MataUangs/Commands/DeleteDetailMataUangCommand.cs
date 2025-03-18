using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.MataUangs.Commands
{
    public class DeleteDetailMataUangCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_mtu { get; set; }

        public DateTime tgl_mul { get; set; }
        
        public DateTime tgl_akh { get; set; }
    }

    public class DeleteDetailMataUangCommandHandler : IRequestHandler<DeleteDetailMataUangCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteDetailMataUangCommandHandler> _logger;

        public DeleteDetailMataUangCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteDetailMataUangCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDetailMataUangCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailMataUang = dbContext.DetailMataUang.FirstOrDefault(w => w.kd_mtu == request.kd_mtu
                    && w.tgl_mul == request.tgl_mul
                    && w.tgl_akh == request.tgl_akh);

                if (detailMataUang != null)
                    dbContext.DetailMataUang.Remove(detailMataUang);

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