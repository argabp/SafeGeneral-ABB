using System;
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
    public class GetKodeTertujuPSTQuery : IRequest<List<DropdownOptionDto>>
    {
    }

    public class GetKodeTertujuPSTQueryHandler : IRequestHandler<GetKodeTertujuPSTQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<GetKodeTertujuPSTQueryHandler> _logger;

        public GetKodeTertujuPSTQueryHandler(IDbConnectionPst connectionPst, ILogger<GetKodeTertujuPSTQueryHandler> logger)
        {
            _connectionPst = connectionPst;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetKodeTertujuPSTQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
                    (await _connectionPst.Query<DropdownOptionDto>(
                        "SELECT RTRIM(LTRIM(kd_grp_rk)) Value, nm_grp_rk Text " +
                        "FROM v_rf02")).ToList()
                , _logger);
        }
    }
}