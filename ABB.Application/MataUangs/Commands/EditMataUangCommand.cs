using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.MataUangs.Commands
{
    public class EditMataUangCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_mtu { get; set; }

        public string nm_mtu { get; set; }

        public string symbol { get; set; }
    }

    public class EditMataUangCommandHandler : IRequestHandler<EditMataUangCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<EditMataUangCommandHandler> _logger;

        public EditMataUangCommandHandler(IDbContextFactory contextFactory,
            ILogger<EditMataUangCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditMataUangCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var mataUang = dbContext.MataUang.FirstOrDefault(w => w.kd_mtu == request.kd_mtu);

                if (mataUang != null)
                {
                    mataUang.nm_mtu = request.nm_mtu;
                    mataUang.symbol = request.symbol;
                }

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