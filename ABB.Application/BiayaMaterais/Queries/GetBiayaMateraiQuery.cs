using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.BiayaMaterais.Queries
{
    public class GetBiayaMateraiQuery : IRequest<BiayaMateraiDto>
    {
        public string DatabaseName { get; set; }
        public string kd_mtu { get; set; }

        public decimal nilai_prm_mul { get; set; }
        
        public decimal nilai_prm_akh { get; set; }
    }

    public class GetBiayaMateraiQueryHandler : IRequestHandler<GetBiayaMateraiQuery, BiayaMateraiDto>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetBiayaMateraiQueryHandler> _logger;

        public GetBiayaMateraiQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetBiayaMateraiQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<BiayaMateraiDto> Handle(GetBiayaMateraiQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<BiayaMateraiDto>(@"SELECT RTRIM(LTRIM(kd_mtu)) + CONVERT(varchar, nilai_prm_mul) + CONVERT(varchar, nilai_prm_akh) Id ,
                                                                        * FROM dp02 WHERE kd_mtu = @kd_mtu AND nilai_prm_mul = @nilai_prm_mul AND
                                                                        nilai_prm_akh = @nilai_prm_akh", new
                {
                    request.kd_mtu, request.nilai_prm_akh, request.nilai_prm_mul
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