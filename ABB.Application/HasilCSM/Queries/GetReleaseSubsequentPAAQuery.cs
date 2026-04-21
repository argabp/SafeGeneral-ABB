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
        public DateTime PeriodeMulai { get; set; }
        
        public DateTime PeriodeAkhir { get; set; }
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
            var periodeMulai = request.PeriodeMulai.ToString("yyyy-MM-dd");
            var periodeAkhir = request.PeriodeAkhir.ToString("yyyy-MM-dd");
            var data = (await _dbConnectionCsm.Query<dynamic>($"SELECT * FROM ReleaseSubsequentPAA WHERE PeriodeProses BETWEEN '{periodeMulai}' AND '{periodeAkhir}'")).ToList();
            
            return JsonConvert.SerializeObject(data);
        }
    }
}