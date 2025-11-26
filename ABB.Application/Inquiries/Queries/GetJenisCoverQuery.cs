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
    public class GetJenisCoverQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }

        public string kd_cb { get; set; }

        public string kd_jns_kr { get; set; }
    }

    public class GetJenisCoverQueryHandler : IRequestHandler<GetJenisCoverQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetJenisCoverQueryHandler> _logger;

        public GetJenisCoverQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetJenisCoverQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetJenisCoverQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>(
                    "SELECT RTRIM(LTRIM(kd_grp_kr)) Value, nm_grp_kr Text " +
                    "FROM rf02d WHERE kd_cb = @kd_cb AND kd_jns_kr = @kd_jns_kr", 
                    new { request.kd_cb, request.kd_jns_kr })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}