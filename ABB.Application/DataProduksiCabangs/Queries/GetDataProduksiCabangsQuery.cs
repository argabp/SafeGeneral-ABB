using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ABB.Application.DataProduksiCabangs.Queries
{
    public class GetDataProduksiCabangsQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }
        
        public DateTime tgl_mul { get; set; }
        public DateTime tgl_akh { get; set; }
    }
    
    public class GetDataProduksiCabangsQueryHandler : IRequestHandler<GetDataProduksiCabangsQuery, string>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<GetDataProduksiCabangsQueryHandler> _logger;

        public GetDataProduksiCabangsQueryHandler(IDbConnectionFactory dbConnectionFactory, ILogger<GetDataProduksiCabangsQueryHandler> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }
    
        public async Task<string> Handle(GetDataProduksiCabangsQuery request, CancellationToken cancellationToken)
        {
            List<dynamic> data = new List<dynamic>();
            try
            {
                _dbConnectionFactory.CreateDbConnection(request.DatabaseName);
                data = (await _dbConnectionFactory.QueryProc<dynamic>("spr_teknik_prd03",
                    new
                    {
                        request.kd_cb, request.kd_cob, request.tgl_mul, request.tgl_akh
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

