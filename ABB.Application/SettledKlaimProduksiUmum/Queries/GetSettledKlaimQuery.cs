using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ABB.Application.SettledKlaimProduksiUmum.Queries
{
    public class GetSettledKlaimQuery : IRequest<string>
    {
        public string kd_cb { get; set; }

        public DateTime tgl_awal { get; set; }

        public DateTime tgl_akhir { get; set; }
    }
    
    public class GetSettledKlaimQueryHandler : IRequestHandler<GetSettledKlaimQuery, string>
    {
        private readonly IDbConnectionCSM _db;
        private readonly ILogger<GetSettledKlaimQueryHandler> _logger;

        public GetSettledKlaimQueryHandler(IDbConnectionCSM db, ILogger<GetSettledKlaimQueryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }
    
        public async Task<string> Handle(GetSettledKlaimQuery request, CancellationToken cancellationToken)
        {
            List<dynamic> data = new List<dynamic>();
            try
            {
                data = (await _db.QueryProc<dynamic>("spr_akturia_01",
                    new
                    {
                        request.kd_cb, request.tgl_awal, request.tgl_akhir
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