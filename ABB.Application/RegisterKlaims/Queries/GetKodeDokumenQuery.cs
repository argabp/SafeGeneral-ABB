using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.RegisterKlaims.Queries
{
    public class GetKodeDokumenQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }

        public string kd_cob { get; set; }
    }

    public class GetKodeDokumenQueryHandler : IRequestHandler<GetKodeDokumenQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKodeDokumenQueryHandler> _logger;

        public GetKodeDokumenQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKodeDokumenQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetKodeDokumenQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>(
                    "SELECT k.kd_dokumen Value, s.nm_dokumenklaim Text " +
                    "FROM MS_DokumenKlaimDetil k " +
                    "INNER JOIN MS_DokumenKlaim s " +
                    "  ON k.kd_cob = s.kd_cob AND s.kd_scob = k.kd_scob WHERE k.kd_cob = @kd_cob", new { request.kd_cob })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}