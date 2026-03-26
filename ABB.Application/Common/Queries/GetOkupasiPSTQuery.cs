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
    public class GetOkupasiPSTQuery : IRequest<List<DropdownOptionDto>>
    {
    }

    public class GetOkupasiPSTQueryHandler : IRequestHandler<GetOkupasiPSTQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionPst _connection;
        private readonly ILogger<GetOkupasiPSTQueryHandler> _logger;

        public GetOkupasiPSTQueryHandler(IDbConnectionPst connection, ILogger<GetOkupasiPSTQueryHandler> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetOkupasiPSTQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>  (await _connection.Query<DropdownOptionDto>("SELECT RTRIM(LTRIM(kd_okup)) Value, nm_okup Text " +
                                                                          "FROM rf11")).ToList(), _logger);
        }
    }
}