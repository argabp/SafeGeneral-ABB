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
    public class GetKelasKonstruksiPSTQuery : IRequest<List<DropdownOptionDto>>
    {
    }

    public class GetKelasKonstruksiPSTQueryHandler : IRequestHandler<GetKelasKonstruksiPSTQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionPst _connection;
        private readonly ILogger<GetKelasKonstruksiPSTQueryHandler> _logger;

        public GetKelasKonstruksiPSTQueryHandler(IDbConnectionPst connection, ILogger<GetKelasKonstruksiPSTQueryHandler> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetKelasKonstruksiPSTQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>  (await _connection.Query<DropdownOptionDto>("SELECT RTRIM(LTRIM(kd_kls_konstr)) Value, nm_kls_konstr Text " +
                                                                          "FROM rf13")).ToList(), _logger);
        }
    }
}