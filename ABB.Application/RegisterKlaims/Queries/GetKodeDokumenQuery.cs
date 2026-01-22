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
        public string kd_scob { get; set; }
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
                    "SELECT k.kd_dokumen Value, s.nm_dokumen Text " +
                    "FROM MS_DokumenKlaimDetil k " +
                    "INNER JOIN MS_DokumenDetil s " +
                    "  ON k.kd_dokumen = s.kd_dokumen WHERE k.kd_cob = @kd_cob AND k.kd_scob = @kd_scob", new { request.kd_cob, request.kd_scob })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}