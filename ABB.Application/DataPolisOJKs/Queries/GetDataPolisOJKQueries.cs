using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ABB.Application.DataPolisOJKs.Queries
{
    public class GetDataPolisOJKQueries : IRequest<string>
    {
        public string kd_cb { get; set; }

        public string jenis_laporan { get; set; }

        public DateTime tgl_akh { get; set; }

        public string DatabaseName { get; set; }
    }
    
    public class GetDataPolisOJKQueriesHandler : IRequestHandler<GetDataPolisOJKQueries, string>
    {
        private readonly IDbConnectionFactory _db;
        private readonly ILogger<GetDataPolisOJKQueriesHandler> _logger;

        public GetDataPolisOJKQueriesHandler(IDbConnectionFactory db, ILogger<GetDataPolisOJKQueriesHandler> logger)
        {
            _db = db;
            _logger = logger;
        }
    
        public async Task<string> Handle(GetDataPolisOJKQueries request, CancellationToken cancellationToken)
        {
            List<dynamic> data = new List<dynamic>();
            try
            {

                var sp = request.jenis_laporan == "1" ? "spr_apolo_03" : "spr_apolo_04";
                
                _db.CreateDbConnection(request.DatabaseName);
                data = (await _db.QueryProc<dynamic>(sp,
                    new
                    {
                        request.kd_cb, request.tgl_akh
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