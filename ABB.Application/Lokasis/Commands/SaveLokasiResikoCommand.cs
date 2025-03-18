using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Lokasis.Commands
{
    public class SaveLokasiResikoCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_pos { get; set; }

        public string jalan { get; set; }

        public string kota { get; set; }
    }

    public class SaveLokasiResikoCommandHandler : IRequestHandler<SaveLokasiResikoCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<SaveLokasiResikoCommandHandler> _logger;

        public SaveLokasiResikoCommandHandler(IDbContextFactory contextFactory,
            ILogger<SaveLokasiResikoCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveLokasiResikoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var lokasiResiko = dbContext.LokasiResiko.FirstOrDefault(w => w.kd_pos == request.kd_pos);

                if (lokasiResiko == null)
                {
                    dbContext.LokasiResiko.Add(new LokasiResiko()
                    {
                        kd_pos = request.kd_pos,
                        jalan = request.jalan,
                        kota = request.kota
                    });
                }
                else
                {
                    lokasiResiko.jalan = request.jalan;
                    lokasiResiko.kota = request.kota;   
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