using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ABB.Application.PolisInfoceProduksiUmum.Queries
{
    public class GetPolisInfoceQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        
        public DateTime tgl_akhir { get; set; }
    }
    
    public class GetPolisInfoceQueryHandler : IRequestHandler<GetPolisInfoceQuery, string>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<GetPolisInfoceQueryHandler> _logger;

        public GetPolisInfoceQueryHandler(IDbConnectionFactory dbConnectionFactory, ILogger<GetPolisInfoceQueryHandler> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }
    
        public async Task<string> Handle(GetPolisInfoceQuery request, CancellationToken cancellationToken)
        {
            List<dynamic> data = new List<dynamic>();
            try
            {
                _dbConnectionFactory.CreateDbConnection(request.DatabaseName);
                data = (await _dbConnectionFactory.QueryProc<dynamic>("spr_akturia_17",
                    new
                    {
                        request.kd_cb, tgl_akh = request.tgl_akhir
                    })).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException == null ? e.Message : e.InnerException.Message);
                throw;
            }

            return JsonConvert.SerializeObject(data);
        }
    }
}