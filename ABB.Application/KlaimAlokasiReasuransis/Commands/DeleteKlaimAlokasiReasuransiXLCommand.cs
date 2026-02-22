using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KlaimAlokasiReasuransis.Commands
{
    public class DeleteKlaimAlokasiReasuransiXLCommand : IRequest
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public short no_mts { get; set; }
        public string kd_jns_sor { get; set; }
        public string kd_kontr { get; set; }
    }

    public class DeleteKlaimAlokasiReasuransiXLCommandHandler : IRequestHandler<DeleteKlaimAlokasiReasuransiXLCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<DeleteKlaimAlokasiReasuransiXLCommandHandler> _logger;

        public DeleteKlaimAlokasiReasuransiXLCommandHandler(IDbContextPst contextPst, 
            ILogger<DeleteKlaimAlokasiReasuransiXLCommandHandler> logger)
        {
            _contextPst = contextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteKlaimAlokasiReasuransiXLCommand request,
            CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var klaimAlokasiReasuransiXL = _contextPst.KlaimAlokasiReasuransiXL.Find(request.kd_cb, request.kd_cob, request.kd_scob, 
                                        request.kd_thn, request.no_kl, request.no_mts, request.kd_jns_sor, request.kd_kontr);

                if (klaimAlokasiReasuransiXL != null)
                {

                    _contextPst.KlaimAlokasiReasuransiXL.Remove(klaimAlokasiReasuransiXL);
                }

                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}