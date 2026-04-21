using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Newtonsoft.Json;

namespace ABB.Application.HasilCSM.Queries
{
    public class GetLiabilityPAAQuery : IRequest<string>
    {
        public DateTime PeriodeAkhir { get; set; }
    }

    public class GetLiabilityPAAQueryHandler : IRequestHandler<GetLiabilityPAAQuery, string>
    {
        private readonly IDbConnectionCSM _dbConnectionCsm;

        public GetLiabilityPAAQueryHandler(IDbConnectionCSM dbConnectionCsm)
        {
            _dbConnectionCsm = dbConnectionCsm;
        }

        public async Task<string> Handle(GetLiabilityPAAQuery request,
            CancellationToken cancellationToken)
        {
            var periode = request.PeriodeAkhir.ToString("yyyy-MM-dd");
            var data = (await _dbConnectionCsm.Query<dynamic>($"SELECT * FROM LiabilityPAA WHERE PeriodeProses = '{periode}'")).ToList();
            
            return JsonConvert.SerializeObject(data);
        }
    }
}