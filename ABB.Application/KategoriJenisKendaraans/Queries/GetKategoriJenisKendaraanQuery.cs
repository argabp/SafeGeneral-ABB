using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KategoriJenisKendaraans.Queries
{
    public class GetKategoriJenisKendaraanQuery : IRequest<KategoriJenisKendaraanDto>
    {
        public string DatabaseName { get; set; }
        public string kd_grp_rsk { get; set; }

        public string kd_rsk { get; set; }
    }

    public class GetKategoriJenisKendaraanQueryHandler : IRequestHandler<GetKategoriJenisKendaraanQuery, KategoriJenisKendaraanDto>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKategoriJenisKendaraanQueryHandler> _logger;

        public GetKategoriJenisKendaraanQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKategoriJenisKendaraanQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<KategoriJenisKendaraanDto> Handle(GetKategoriJenisKendaraanQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<KategoriJenisKendaraanDto>(@"SELECT * FROM rf10d WHERE kd_grp_rsk = @kd_grp_rsk AND kd_rsk = @kd_rsk", new
                {
                    request.kd_grp_rsk, request.kd_rsk
                })).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}