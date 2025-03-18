using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetKodeKecamatanQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }

        public string kd_prop { get; set; }

        public string kd_kab { get; set; }
    }

    public class GetKodeKecamatanQueryHandler : IRequestHandler<GetKodeKecamatanQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKodeKecamatanQueryHandler> _logger;

        public GetKodeKecamatanQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKodeKecamatanQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetKodeKecamatanQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>("SELECT RTRIM(LTRIM(kd_kec)) Value, nm_kec Text " +
                                                                          "FROM rf07_02 WHERE kd_prop = @kd_prop AND kd_kab = @kd_kab", 
                                                                        new { request.kd_prop, request.kd_kab })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}