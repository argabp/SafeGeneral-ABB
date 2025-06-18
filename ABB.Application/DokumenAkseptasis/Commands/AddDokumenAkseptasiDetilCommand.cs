using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.DokumenAkseptasis.Commands
{
    public class AddDokumenAkseptasiDetilCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public Int16 kd_dokumen { get; set; }

        public bool flag_wajib { get; set; }
    }

    public class AddDokumenAkseptasiDetilCommandHandler : IRequestHandler<AddDokumenAkseptasiDetilCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly ILogger<AddDokumenAkseptasiDetilCommandHandler> _logger;

        public AddDokumenAkseptasiDetilCommandHandler(IDbContextFactory contextFactory,
            ILogger<AddDokumenAkseptasiDetilCommandHandler> logger)
        {;
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddDokumenAkseptasiDetilCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var dokumenAkseptasiDetil = new DokumenAkseptasiDetil()
                {
                    kd_cob = request.kd_cob,
                    kd_scob = request.kd_scob,
                    kd_dokumen = request.kd_dokumen,
                    flag_wajib = request.flag_wajib
                };

                dbContext.DokumenAkseptasiDetil.Add(dokumenAkseptasiDetil);

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