using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PLAReasuransis.Commands
{
    public class SavePLAReasuransiCommand : IRequest
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public short no_mts { get; set; }

        public short no_pla { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }

        public string? ket_pla { get; set; }
    }

    public class SavePLAReasuransiCommandHandler : IRequestHandler<SavePLAReasuransiCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<SavePLAReasuransiCommandHandler> _logger;

        public SavePLAReasuransiCommandHandler(IDbContextPst contextPst,
            ILogger<SavePLAReasuransiCommandHandler> logger, IMapper mapper)
        {;
            _contextPst = contextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(SavePLAReasuransiCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
            {
                var plaReasuransi = _contextPst.PLAReasuransi.Find(request.kd_cb, request.kd_cob, request.kd_scob,
                    request.kd_thn, request.no_kl, request.no_mts, request.no_pla);

                plaReasuransi.ket_pla = request.ket_pla;

                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}