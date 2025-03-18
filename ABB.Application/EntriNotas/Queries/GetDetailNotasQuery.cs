using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.EntriNotas.Queries
{
    public class GetDetailNotasQuery : IRequest<List<DetailNotaDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
        public string kd_cb { get; set; }

        public string jns_tr { get; set; }

        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }
    }

    public class GetDetailNotasQueryHandler : IRequestHandler<GetDetailNotasQuery, List<DetailNotaDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetDetailNotasQueryHandler> _logger;

        public GetDetailNotasQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetDetailNotasQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DetailNotaDto>> Handle(GetDetailNotasQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);
            
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DetailNotaDto>("SELECT * FROM uw08ed WHERE " +
                                                                      "kd_cb = @kd_cb AND jns_tr = @jns_tr AND " +
                                                                      "jns_nt_msk = @jns_nt_msk AND kd_thn = @kd_thn AND " +
                                                                      "kd_bln = @kd_bln AND no_nt_msk = @no_nt_msk AND " +
                                                                      "jns_nt_kel = @jns_nt_kel AND no_nt_kel = @no_nt_kel",
                    new
                    {
                        request.kd_cb, request.jns_tr, request.jns_nt_msk, request.kd_thn,
                        request.kd_bln, request.no_nt_msk, request.jns_nt_kel, request.no_nt_kel
                    })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}