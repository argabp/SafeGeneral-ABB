using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KlaimAlokasiReasuransis.Commands
{
    public class SaveMutasiKlaimCommand : IRequest
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public short no_mts { get; set; }
        public DateTime tgl_reas { get; set; }
    }

    public class SaveMutasiKlaimCommandHandler : IRequestHandler<SaveMutasiKlaimCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<SaveMutasiKlaimCommandHandler> _logger;

        public SaveMutasiKlaimCommandHandler(IDbContextPst contextPst,
            ILogger<SaveMutasiKlaimCommandHandler> logger)
        {
            _contextPst = contextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveMutasiKlaimCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var mutasiKlaim = _contextPst.MutasiKlaim.Find(request.kd_cb, request.kd_cob, request.kd_scob, 
                                        request.kd_thn, request.no_kl, request.no_mts);

                if (mutasiKlaim != null)
                {
                    mutasiKlaim.tgl_reas = request.tgl_reas;
                }

                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}