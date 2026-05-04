using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.NotaPremiTreatyKeluars.Commands
{
    public class SaveNotaPremiTreatyKeluarCommand : IRequest
    {
        public string kd_cb { get; set; }

        public string jns_tr { get; set; }

        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }

        public string? ket_nt { get; set; }
    }

    public class SaveNotaPremiTreatyKeluarCommandHandler : IRequestHandler<SaveNotaPremiTreatyKeluarCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<SaveNotaPremiTreatyKeluarCommandHandler> _logger;

        public SaveNotaPremiTreatyKeluarCommandHandler(IDbContextPst contextPst,
            ILogger<SaveNotaPremiTreatyKeluarCommandHandler> logger)
        {;
            _contextPst = contextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveNotaPremiTreatyKeluarCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var nota = _contextPst.NotaPremiTreatyKeluar.Find(request.kd_cb, request.jns_tr, request.jns_nt_msk,
                    request.kd_thn, request.kd_bln, request.no_nt_msk, request.jns_nt_kel, request.no_nt_kel);

                nota.ket_nt = request.ket_nt;
                
                await _contextPst.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }, _logger);
        }
    }
}