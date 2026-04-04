using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.NotaTreatyMasukXOLs.Commands
{
    public class SaveNotaCommand : IRequest, IMapFrom<NotaTreatyMasuk>
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

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveNotaCommand, NotaTreatyMasuk>();
        }
    }

    public class SaveNotaCommandHandler : IRequestHandler<SaveNotaCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<SaveNotaCommandHandler> _logger;

        public SaveNotaCommandHandler(IDbContextPst contextPst,
            ILogger<SaveNotaCommandHandler> logger)
        {;
            _contextPst = contextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveNotaCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var notaTreatyMasuk =
                    _contextPst.NotaTreatyMasuk.Find(request.kd_cb, request.jns_tr, request.jns_nt_msk, request.kd_thn,
                        request.kd_bln, request.no_nt_msk, request.jns_nt_kel, request.no_nt_kel);
                
                if (notaTreatyMasuk != null)
                {
                    notaTreatyMasuk.ket_nt = request.ket_nt;
                }

                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}