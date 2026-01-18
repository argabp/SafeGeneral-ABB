using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ApprovalMutasiKlaims.Queries
{
    public class GetUserSignRevisedQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }

        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }
    }

    public class GetUserSignRevisedQueryHandler : IRequestHandler<GetUserSignRevisedQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetUserSignRevisedQueryHandler> _logger;

        public GetUserSignRevisedQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetUserSignRevisedQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetUserSignRevisedQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>(@"SELECT DISTINCT kd_user Value, nm_user_sign Text 
                                                                          FROM v_user_rev_cl WHERE kd_cb = @kd_cb AND 
                                                                                            kd_cob = @kd_cob AND
                                                                                              kd_scob = @kd_scob AND
                                                                                              kd_thn = @kd_thn AND
                                                                                              no_kl = @no_kl",
                    new { request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn, request.no_kl })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}