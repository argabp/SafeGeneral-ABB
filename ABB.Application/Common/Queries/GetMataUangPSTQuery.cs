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
    public class GetMataUangPSTQuery : IRequest<List<DropdownOptionDto>>
    {
    }

    public class GetMataUangPSTQueryHandler : IRequestHandler<GetMataUangPSTQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<GetMataUangPSTQueryHandler> _logger;

        public GetMataUangPSTQueryHandler(IDbConnectionPst connectionPst, 
            ILogger<GetMataUangPSTQueryHandler> logger)
        {
            _connectionPst = connectionPst;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetMataUangPSTQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                (await _connectionPst.Query<DropdownOptionDto>("SELECT m.kd_mtu Value, m.nm_mtu + '(' + m.symbol + ')' Text FROM rf06 m"))
                .ToList(), _logger);
        }
    }
}