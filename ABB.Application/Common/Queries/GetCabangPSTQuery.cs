using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Common.Queries
{
    public class GetCabangPSTQuery : IRequest<List<DropdownOptionDto>>
    {
    }

    public class GetCabangPSTQueryHandler : IRequestHandler<GetCabangPSTQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionPst _connection;
        private readonly ILogger<GetCabangPSTQueryHandler> _logger;

        public GetCabangPSTQueryHandler(IDbConnectionPst connection, ILogger<GetCabangPSTQueryHandler> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetCabangPSTQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                return (await _connection.Query<DropdownOptionDto>("SELECT RTRIM(LTRIM(kd_cb)) Value, nm_cb Text " +
                                                                          "FROM rf01")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}