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

namespace ABB.Application.Akseptasis.Queries
{
    public class GetKodeKelurahanQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }

        public string kd_prop { get; set; }

        public string kd_kab { get; set; }

        public string kd_kec { get; set; }
    }

    public class GetKodeKelurahanQueryHandler : IRequestHandler<GetKodeKelurahanQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKodeKelurahanQueryHandler> _logger;

        public GetKodeKelurahanQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKodeKelurahanQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetKodeKelurahanQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>("SELECT RTRIM(LTRIM(kd_kel)) Value, nm_kel Text " +
                                                                          "FROM rf07_03 WHERE kd_prop = @kd_prop AND " +
                                                                          "kd_kab = @kd_kab AND kd_kec = @kd_kec", 
                    new { request.kd_prop, request.kd_kab, request.kd_kec })).ToList();
            }, _logger);
        }
    }
}