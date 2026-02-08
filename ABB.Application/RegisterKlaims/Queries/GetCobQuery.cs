using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.RegisterKlaims.Queries
{
    public class GetCobQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetCobQueryHandler : IRequestHandler<GetCobQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetCobQueryHandler> _logger;

        public GetCobQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetCobQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetCobQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>("SELECT RTRIM(LTRIM(kd_cob)) Value, nm_cob Text " +
                                                                          "FROM rf04 WHERE kd_cob <> 'X'")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}