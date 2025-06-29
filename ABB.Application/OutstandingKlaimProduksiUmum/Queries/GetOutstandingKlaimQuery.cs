using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ABB.Application.OutstandingKlaimProduksiUmum.Queries
{
    public class GetOutstandingKlaimQuery : IRequest<string>
    {
        public string kd_cb { get; set; }

        public DateTime tgl_akhir { get; set; }
    }
    
    public class GetOutstandingKlaimQueryHandler : IRequestHandler<GetOutstandingKlaimQuery, string>
    {
        private readonly IDbConnectionCSM _db;
        private readonly ILogger<GetOutstandingKlaimQueryHandler> _logger;

        public GetOutstandingKlaimQueryHandler(IDbConnectionCSM db, ILogger<GetOutstandingKlaimQueryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }
    
        public async Task<string> Handle(GetOutstandingKlaimQuery request, CancellationToken cancellationToken)
        {
            List<dynamic> data = new List<dynamic>();
            try
            {
                data = (await _db.QueryProc<dynamic>("spr_akturia_02",
                    new
                    {
                        request.kd_cb, request.tgl_akhir
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