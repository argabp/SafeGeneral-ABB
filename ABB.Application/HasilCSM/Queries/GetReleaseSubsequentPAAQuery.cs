using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Newtonsoft.Json;

namespace ABB.Application.HasilCSM.Queries
{
    public class GetReleaseSubsequentPAAQuery : IRequest<string>
    {
        public string TipeTransaksi { get; set; }
        
        public DateTime Periode { get; set; }
    }

    public class GetReleaseSubsequentPAAQueryHandler : IRequestHandler<GetReleaseSubsequentPAAQuery, string>
    {
        private readonly IDbConnectionCSM _dbConnectionCsm;

        public GetReleaseSubsequentPAAQueryHandler(IDbConnectionCSM dbConnectionCsm)
        {
            _dbConnectionCsm = dbConnectionCsm;
        }

        public async Task<string> Handle(GetReleaseSubsequentPAAQuery request,
            CancellationToken cancellationToken)
        {
            var periode = request.Periode.ToString("yyyy-MM-dd");
            var data = (await _dbConnectionCsm.Query<dynamic>($"SELECT * FROM ReleaseSubsequentPAA WHERE TipeTransaksi = '{request.TipeTransaksi}' AND PeriodeProses = '{periode}'")).ToList();
            
            return JsonConvert.SerializeObject(data);
        }
    }
}