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
    public class GetKodeKelasKonstruksiQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetKodeKelasKonstruksiQueryHandler : IRequestHandler<GetKodeKelasKonstruksiQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKodeKelasKonstruksiQueryHandler> _logger;

        public GetKodeKelasKonstruksiQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKodeKelasKonstruksiQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetKodeKelasKonstruksiQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>("SELECT RTRIM(LTRIM(kd_kls_konstr)) Value, nm_kls_konstr Text " +
                                                                          "FROM rf13")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}