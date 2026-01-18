using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.ApprovalKlaims.Queries;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ApprovalKlaims.Queries
{
    public class GetApprovalKlaimDetailsQuery : IRequest<List<ApprovalKlaimDetailDto>>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }
    }

    public class GetApprovalKlaimDetailsQueryHandler : IRequestHandler<GetApprovalKlaimDetailsQuery, List<ApprovalKlaimDetailDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetApprovalKlaimDetailsQueryHandler> _logger;

        public GetApprovalKlaimDetailsQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetApprovalKlaimDetailsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<ApprovalKlaimDetailDto>> Handle(GetApprovalKlaimDetailsQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);

            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var results = (await _connectionFactory.Query<ApprovalKlaimDetailDto>(@"SELECT ad.*, s.nm_status, ISNULL(u1.FirstName, '') + ' ' + ISNULL(u1.LastName, '') nm_user_sign, ISNULL(u2.FirstName, '') + ' ' + ISNULL(u2.LastName, '') nm_user FROM MS_ApprovalKlaimDetilSign ad 
                                INNER JOIN MS_User u1 
                                    ON u1.UserId = ad.kd_user_sign
                                INNER JOIN MS_User u2 
                                    ON u2.UserId = ad.kd_user
                                INNER JOIN MS_Status s 
                                    ON s.kd_status = ad.kd_status
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