using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Newtonsoft.Json;

namespace ABB.Application.HasilCSM.Queries
{
    public class GetReleaseSubsequentGMMRekapQuery : IRequest<string>
    {
        public DateTime PeriodeMulai { get; set; }
        
        public DateTime PeriodeAkhir { get; set; }
    }

    public class GetReleaseSubsequentGMMRekapQueryHandler : IRequestHandler<GetReleaseSubsequentGMMRekapQuery, string>
    {
        private readonly IDbConnectionCSM _dbConnectionCsm;

        public GetReleaseSubsequentGMMRekapQueryHandler(IDbConnectionCSM dbConnectionCsm)
        {
            _dbConnectionCsm = dbConnectionCsm;
        }

        public async Task<string> Handle(GetReleaseSubsequentGMMRekapQuery request,
            CancellationToken cancellationToken)
        {
            var periodeMulai = request.PeriodeMulai.ToString("yyyy-MM-dd");
            var periodeAkhir = request.PeriodeAkhir.ToString("yyyy-MM-dd");
            var data = (await _dbConnectionCsm.Query<dynamic>($"SELECT * FROM ReleaseSubsequentGMMRekap WHERE PeriodeProses BETWEEN '{periodeMulai}' AND '{periodeAkhir}'")).ToList();
            
            return JsonConvert.SerializeObject(data);
        }
    }
}