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
    public class GetRekanansByKodeGroupAndCabangPSTQuery : IRequest<List<DropdownOptionDto>>
    {
        public string kd_cb { get; set; }
        public string kd_grp_rk { get; set; }
    }

    public class GetRekanansByKodeGroupAndCabangPSTQueryHandler : IRequestHandler<GetRekanansByKodeGroupAndCabangPSTQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<GetRekanansByKodeGroupAndCabangPSTQueryHandler> _logger;

        public GetRekanansByKodeGroupAndCabangPSTQueryHandler(IDbConnectionPst connectionPst, ILogger<GetRekanansByKodeGroupAndCabangPSTQueryHandler> logger)
        {
            _connectionPst = connectionPst;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetRekanansByKodeGroupAndCabangPSTQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => (await _connectionPst.Query<DropdownOptionDto>(
                "SELECT kd_rk Value, nm_rk Text FROM rf03 WHERE kd_grp_rk = @kd_grp_rk AND kd_cb = @kd_cb",
                new { request.kd_grp_rk, request.kd_cb })).ToList(), _logger);
        }
    }
}