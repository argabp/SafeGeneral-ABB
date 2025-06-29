using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ABB.Application.NotaResiko.Queries
{
    public class GetSourceDataQuery : IRequest<string>
    {
        public string TipeTransaksi { get; set; }

        public string KodeMetode { get; set; }

        public DateTime PeriodeAwal { get; set; }
        public DateTime PeriodeAkhir { get; set; }

        public bool FlagRelease { get; set; }
    }

    public class GetSourceDataQueryHandler : IRequestHandler<GetSourceDataQuery, string>
    {
        private readonly IDbConnectionCSM _dbConnectionCsm;
        private readonly ILogger<GetSourceDataQueryHandler> _logger;

        public GetSourceDataQueryHandler(IDbConnectionCSM dbConnectionCsm, ILogger<GetSourceDataQueryHandler> logger)
        {
            _dbConnectionCsm = dbConnectionCsm;
            _logger = logger;
        }

        public async Task<string> Handle(GetSourceDataQuery request,
            CancellationToken cancellationToken)
        {
            List<dynamic> data = new List<dynamic>();
            int flagRelease = request.FlagRelease ? 1 : 0;
            try
            {
                var script = $@"SELECT * FROM MS_SourceData WHERE KodeMetode = {request.KodeMetode} 
                            AND FlagRelease = {flagRelease} AND TipeTransaksi = '{request.TipeTransaksi}' AND TglTransaksi 
                            BETWEEN '{request.PeriodeAwal:yyyy-MM-dd}' AND '{request.PeriodeAkhir:yyyy-MM-dd}'";
                
                data = (await _dbConnectionCsm.Query<dynamic>(script)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

            return JsonConvert.SerializeObject(data);
        }
    }
}