using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Common.Queries
{
    public class GetUserSignEscKlaimQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
    }

    public class GetUserSignEscKlaimQueryHandler : IRequestHandler<GetUserSignEscKlaimQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetUserSignEscKlaimQueryHandler> _logger;

        public GetUserSignEscKlaimQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetUserSignEscKlaimQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetUserSignEscKlaimQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>("SELECT DISTINCT kd_user_sign Value, nm_user_sign Text " +
                                                                          "FROM v_user_esc_cl WHERE kd_cb = @kd_cb", new { request.kd_cb })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}