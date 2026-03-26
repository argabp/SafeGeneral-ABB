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
    public class GetCoveragePSTQuery : IRequest<List<DropdownOptionDto>>
    {
    }

    public class GetCoveragePSTQueryHandler : IRequestHandler<GetCoveragePSTQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionPst _connection;
        private readonly ILogger<GetCoveragePSTQueryHandler> _logger;

        public GetCoveragePSTQueryHandler(IDbConnectionPst connection, ILogger<GetCoveragePSTQueryHandler> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetCoveragePSTQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>  (await _connection.Query<DropdownOptionDto>("SELECT RTRIM(LTRIM(kd_cvrg)) Value, nm_cvrg Text " +
                                                                          "FROM rf17")).ToList(), _logger);
        }
    }
}