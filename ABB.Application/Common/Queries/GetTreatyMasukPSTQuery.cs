using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Common.Queries
{
    
    public class GetTreatyMasukPSTQuery : IRequest<List<DropdownOptionDto>>
    {
        public string kd_cb { get; set; }
        public string kd_jns_sor { get; set; }
    }

    public class GetTreatyMasukPSTQueryHandler : IRequestHandler<GetTreatyMasukPSTQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<GetTreatyMasukPSTQueryHandler> _logger;

        public GetTreatyMasukPSTQueryHandler(IDbConnectionPst connectionPst, ILogger<GetTreatyMasukPSTQueryHandler> logger)
        {
            _connectionPst = connectionPst;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetTreatyMasukPSTQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => (await _connectionPst.Query<DropdownOptionDto>(
                "SELECT kd_tty_msk Value, desk_tty Text FROM ri01i WHERE kd_jns_sor = @kd_jns_sor AND kd_cb = @kd_cb",
                new { request.kd_jns_sor, request.kd_cb })).ToList(), _logger);
        }
    }
}