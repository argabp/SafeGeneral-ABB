using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Lokasis.Queries
{
    public class GetDetailLokasiResikoQuery : IRequest<List<DetailLokasiResikoDto>>
    {
        public string DatabaseName { get; set; }
        public string kd_pos { get; set; }
    }

    public class GetDetailLokasiResikoQueryHandler : IRequestHandler<GetDetailLokasiResikoQuery, List<DetailLokasiResikoDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetDetailLokasiResikoQueryHandler> _logger;

        public GetDetailLokasiResikoQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetDetailLokasiResikoQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DetailLokasiResikoDto>> Handle(GetDetailLokasiResikoQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DetailLokasiResikoDto>(@"SELECT detail.kd_pos + detail.kd_lok_rsk AS Id, detail.kd_pos, detail.kd_lok_rsk,
		                                                                        detail.gedung, detail.alamat, detail.kd_prop, detail.kd_kab, detail.kd_kec,
		                                                                        detail.kd_kel, provinsi.nm_prop, kabupaten.nm_kab, kecamatan.nm_kec, kelurahan.nm_kel
	                                                                        FROM rf25d detail
		                                                                        LEFT JOIN rf07 provinsi
			                                                                        ON provinsi.kd_prop = detail.kd_prop
		                                                                        LEFT JOIN rf07_01 kabupaten
			                                                                        ON kabupaten.kd_kab = detail.kd_kab
				                                                                        AND kabupaten.kd_prop = provinsi.kd_prop
		                                                                        LEFT JOIN rf07_02 kecamatan
			                                                                        ON kecamatan.kd_kec = detail.kd_kec
		                                                                        LEFT JOIN rf07_03 kelurahan
			                                                                        ON kelurahan.kd_kel = detail.kd_kel
		                                                                        WHERE kd_pos = @kd_pos",
                    new { request.kd_pos })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}