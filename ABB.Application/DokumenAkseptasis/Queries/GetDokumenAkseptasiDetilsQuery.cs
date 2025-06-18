using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.DokumenAkseptasis.Queries
{
    public class GetDokumenAkseptasiDetilsQuery : IRequest<List<DokumenAkseptasiDetilDto>>
    {
        public string DatabaseName { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }
    }

    public class GetDokumenAkseptasiDetilsQueryHandler : IRequestHandler<GetDokumenAkseptasiDetilsQuery, List<DokumenAkseptasiDetilDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetDokumenAkseptasiDetilsQueryHandler> _logger;

        public GetDokumenAkseptasiDetilsQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetDokumenAkseptasiDetilsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DokumenAkseptasiDetilDto>> Handle(GetDokumenAkseptasiDetilsQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);

            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var results = (await _connectionFactory.Query<DokumenAkseptasiDetilDto>(@"SELECT da.*, dd.* FROM MS_DokumenAkseptasiDetil da 
                                INNER JOIN MS_DokumenDetil dd 
                                    ON dd.kd_dokumen = da.kd_dokumen
                                WHERE kd_cob = @kd_cob AND kd_scob = @kd_scob",
                    new { request.kd_cob, request.kd_scob })).ToList();

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