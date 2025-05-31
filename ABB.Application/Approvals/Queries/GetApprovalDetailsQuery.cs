using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Approvals.Queries
{
    public class GetApprovalDetailsQuery : IRequest<List<ApprovalDetailDto>>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }
    }

    public class GetApprovalDetailsQueryHandler : IRequestHandler<GetApprovalDetailsQuery, List<ApprovalDetailDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetApprovalDetailsQueryHandler> _logger;

        public GetApprovalDetailsQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetApprovalDetailsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<ApprovalDetailDto>> Handle(GetApprovalDetailsQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);

            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var results = (await _connectionFactory.Query<ApprovalDetailDto>(@"SELECT ad.*, ISNULL(u.FirstName, '') + ' ' + ISNULL(u.LastName, '') nm_user_sign FROM MS_ApprovalDetil ad 
                                INNER JOIN MS_User u 
                                    ON u.UserId = ad.kd_user_sign
                                WHERE kd_cb = @kd_cb 
                                  AND kd_cob = @kd_cob AND kd_scob = @kd_scob",
                    new { request.kd_cb, request.kd_cob, request.kd_scob })).ToList();

                var sequence = 0;
                foreach (var result in results)
                {
                    sequence++;
                    result.Id = sequence;
                }

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}