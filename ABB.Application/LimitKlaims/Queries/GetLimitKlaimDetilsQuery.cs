using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.LimitKlaims.Queries
{
    public class GetLimitKlaimDetilsQuery : IRequest<List<LimitKlaimDetilDto>>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }
    }

    public class GetLimitKlaimDetilsQueryHandler : IRequestHandler<GetLimitKlaimDetilsQuery, List<LimitKlaimDetilDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetLimitKlaimDetilsQueryHandler> _logger;

        public GetLimitKlaimDetilsQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetLimitKlaimDetilsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<LimitKlaimDetilDto>> Handle(GetLimitKlaimDetilsQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);

            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var results = (await _connectionFactory.Query<LimitKlaimDetilDto>(@"SELECT ad.*, ISNULL(u2.FirstName, '') + ' ' + ISNULL(u2.LastName, '') nm_user FROM MS_LimitKlaimDetil ad 
                                INNER JOIN MS_User u2 
                                    ON u2.UserId = ad.kd_user
                                WHERE kd_cb = @kd_cb AND thn = @thn
                                  AND kd_cob = @kd_cob AND kd_scob = @kd_scob",
                    new { request.kd_cb, request.kd_cob, request.kd_scob, request.thn })).ToList();

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