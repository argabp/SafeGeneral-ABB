using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Inquiries.Queries
{
    public class GetKodeTOLQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }

        public string kd_cob { get; set; }
    }

    public class GetKodeTOLQueryHandler : IRequestHandler<GetKodeTOLQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKodeTOLQueryHandler> _logger;

        public GetKodeTOLQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKodeTOLQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetKodeTOLQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>("SELECT RTRIM(LTRIM(kd_tol)) Value, nm_tol Text " +
                                                                          "FROM rf48 WHERE kd_cob = @kd_cob", new { request.kd_cob })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}