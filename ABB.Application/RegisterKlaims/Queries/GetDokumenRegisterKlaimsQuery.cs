using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ABB.Application.RegisterKlaims.Queries
{
    public class GetDokumenRegisterKlaimsQuery : IRequest<List<DokumenRegisterKlaimDto>>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }
    }

    public class GetDokumenRegisterKlaimsQueryHandler : IRequestHandler<GetDokumenRegisterKlaimsQuery, List<DokumenRegisterKlaimDto>>
    {
        private readonly IDbConnectionFactory _db;

        public GetDokumenRegisterKlaimsQueryHandler(IDbConnectionFactory db, ILogger<GetDokumenRegisterKlaimsQueryHandler> logger
            ,IHostEnvironment host)
        {
            _db = db;
        }

        public async Task<List<DokumenRegisterKlaimDto>> Handle(GetDokumenRegisterKlaimsQuery request,
            CancellationToken cancellationToken)
        {
            _db.CreateDbConnection(request.DatabaseName);
            var lampirans = (await _db.Query<DokumenRegisterKlaimDto>(
                @$"SELECT a.*, d.nm_dokumen dokumenName FROM cl01d a
                                INNER JOIN dp20 d
                                    ON a.kd_dok = d.kd_dokumen
                                   WHERE a.kd_cb = '{request.kd_cb}' AND 
                               a.kd_cob = '{request.kd_cob}' AND a.kd_scob = '{request.kd_scob}' AND a.kd_thn = '{request.kd_thn}' AND
                               a.no_kl = '{request.no_kl}'")).ToList();

            return lampirans;
        } 
    }
}