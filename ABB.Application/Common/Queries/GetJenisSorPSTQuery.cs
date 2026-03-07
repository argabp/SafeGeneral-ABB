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
    public class GetJenisSorPSTQuery : IRequest<List<DropdownOptionDto>>
    {
    }

    public class GetJenisSorPSTQueryHandler : IRequestHandler<GetJenisSorPSTQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<GetJenisSorPSTQueryHandler> _logger;

        public GetJenisSorPSTQueryHandler(IDbConnectionPst connectionPst,
            ILogger<GetJenisSorPSTQueryHandler> logger)
        {
            _connectionPst = connectionPst;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetJenisSorPSTQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
                    (await _connectionPst.Query<DropdownOptionDto>(
                        "SELECT kd_jns_sor Value, nm_jns_sor Text FROM rf18")).ToList()
                , _logger);
        }
    }
}