using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.MataUangs.Commands
{
    public class AddMataUangCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_mtu { get; set; }

        public string nm_mtu { get; set; }

        public string symbol { get; set; }
    }

    public class AddMataUangCommandHandler : IRequestHandler<AddMataUangCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<AddMataUangCommandHandler> _logger;

        public AddMataUangCommandHandler(IDbContextFactory contextFactory,
            ILogger<AddMataUangCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddMataUangCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var mataUang = new MataUang()
                {
                    kd_mtu = request.kd_mtu,
                    nm_mtu = request.nm_mtu,
                    symbol = request.symbol
                };

                dbContext.MataUang.Add(mataUang);

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