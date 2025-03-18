using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PertanggunganKendaraans.Queries
{
    public class GetDetailPertanggunganKendaraanQuery : IRequest<DetailPertanggunganKendaraanDto>
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_jns_ptg { get; set; }

        public short no_urut { get; set; }
    }

    public class GetDetailPertanggunganKendaraanQueryHandler : IRequestHandler<GetDetailPertanggunganKendaraanQuery, DetailPertanggunganKendaraanDto>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetDetailPertanggunganKendaraanQueryHandler> _logger;

        public GetDetailPertanggunganKendaraanQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetDetailPertanggunganKendaraanQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<DetailPertanggunganKendaraanDto> Handle(GetDetailPertanggunganKendaraanQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);

            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DetailPertanggunganKendaraanDto>("SELECT d.* FROM dp01d d WHERE" +
                " d.kd_cob = @kd_cob AND d.kd_scob = @kd_scob AND d.kd_jns_ptg = @kd_jns_ptg AND d.no_urut = @no_urut",
                new { request.kd_cob, request.kd_scob, request.kd_jns_ptg, request.no_urut })).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}