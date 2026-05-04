using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.NotaPremiTreatyKeluars.Queries
{
    public class GetKontrakTreatyKeluarQuery : IRequest<List<DropdownOptionDto>>
    {
        public string kd_cb { get; set; }

        public string kd_jns_tty { get; set; }
    }

    public class GetKontrakTreatyKeluarQueryHandler : IRequestHandler<GetKontrakTreatyKeluarQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<GetKontrakTreatyKeluarQueryHandler> _logger;

        public GetKontrakTreatyKeluarQueryHandler(IDbConnectionPst connectionPst,
            ILogger<GetKontrakTreatyKeluarQueryHandler> logger)
        {
            _connectionPst = connectionPst;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetKontrakTreatyKeluarQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
                    (await _connectionPst.Query<DropdownOptionDto>(
                        "SELECT kd_tty_pps Value, nm_tty_pps Text FROM ri01t WHERE kd_cb = @kd_cb AND kd_jns_sor = @kd_jns_tty", new { request.kd_cb, request.kd_jns_tty })).ToList()
                , _logger);
        }
    }
}