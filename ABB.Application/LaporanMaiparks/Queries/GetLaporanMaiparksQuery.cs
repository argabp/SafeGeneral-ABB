using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Newtonsoft.Json;

namespace ABB.Application.LaporanMaiparks.Queries
{
    public class GetLaporanMaiparksQuery : IRequest<string>
    {
        public DateTime tgl_mul { get; set; }

        public DateTime tgl_akh { get; set; }
    }

    public class GetLaporanMaiparksQueryHandler : IRequestHandler<GetLaporanMaiparksQuery, string>
    {
        private readonly IDbConnectionPst _dbConnectionPst;

        public GetLaporanMaiparksQueryHandler(IDbConnectionPst dbConnectionPst)
        {
            _dbConnectionPst = dbConnectionPst;
        }

        public async Task<string> Handle(GetLaporanMaiparksQuery request,
            CancellationToken cancellationToken)
        {
            var data = (await _dbConnectionPst.QueryProc<dynamic>("spr_ri31r_05", new
            {
                input_str = $",F,{request.tgl_mul},{request.tgl_akh}"
            })).ToList();
            
            return JsonConvert.SerializeObject(data);
        }
    }
}