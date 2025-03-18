using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.EntriNotas.Commands
{
    public class DeleteDetailNotaCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string jns_tr { get; set; }

        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }

        public byte no_ang { get; set; }
    }

    public class DeleteDetailNotaCommandHandler : IRequestHandler<DeleteDetailNotaCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<DeleteDetailNotaCommandHandler> _logger;

        public DeleteDetailNotaCommandHandler(IDbContextFactory contextFactory, ILogger<DeleteDetailNotaCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDetailNotaCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailNota = dbContext.DetailNota.FirstOrDefault(w =>
                    w.kd_cb == request.kd_cb && w.jns_tr == request.jns_tr &&
                    w.jns_nt_msk == request.jns_nt_msk && w.kd_thn == request.kd_thn &&
                    w.kd_bln == request.kd_bln && w.no_nt_msk == request.no_nt_msk &&
                    w.jns_nt_kel == request.jns_nt_kel && w.no_nt_kel == request.no_nt_kel &&
                    w.no_ang == request.no_ang);

                if (detailNota != null)
                    dbContext.DetailNota.Remove(detailNota);

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