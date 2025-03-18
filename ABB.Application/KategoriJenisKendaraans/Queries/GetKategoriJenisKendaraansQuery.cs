using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KategoriJenisKendaraans.Queries
{
    public class GetKategoriJenisKendaraansQuery : IRequest<List<KategoriJenisKendaraanDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetKategoriJenisKendaraansQueryHandler : IRequestHandler<GetKategoriJenisKendaraansQuery, List<KategoriJenisKendaraanDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKategoriJenisKendaraansQueryHandler> _logger;

        public GetKategoriJenisKendaraansQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKategoriJenisKendaraansQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<KategoriJenisKendaraanDto>> Handle(GetKategoriJenisKendaraansQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<KategoriJenisKendaraanDto>(@"SELECT RTRIM(LTRIM(b.kd_grp_rsk)) + RTRIM(LTRIM(b.kd_rsk)) Id ,
                                                                        b.*, m.desk_grp_rsk FROM rf10d b INNER JOIN rf10 m ON b.kd_grp_rsk = m.kd_grp_rsk 
                                                                        WHERE b.kd_grp_rsk = '001'")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}