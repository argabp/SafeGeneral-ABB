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
        public DateTime tgl_akhir { get; set; }
    }
    
    public class GetPolisInfoceQueryHandler : IRequestHandler<GetPolisInfoceQuery, string>
    {
        private readonly IDbConnectionCSM _db;
        private readonly ILogger<GetPolisInfoceQueryHandler> _logger;

        public GetPolisInfoceQueryHandler(IDbConnectionCSM db, ILogger<GetPolisInfoceQueryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }
    
        public async Task<string> Handle(GetPolisInfoceQuery request, CancellationToken cancellationToken)
        {
            List<dynamic> data = new List<dynamic>();
            try
            {
                data = (await _db.QueryProc<dynamic>("spr_akturia_03",
                    new
                    {
                        tgl_akh = request.tgl_akhir
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