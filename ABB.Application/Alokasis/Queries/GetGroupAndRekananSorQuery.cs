using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Alokasis.Queries
{
    public class GetGroupAndRekananSorQuery : IRequest<string>
    {
        public string kd_jns_sor { get; set; }
        public string kd_cob { get; set; }
        public string kd_cb { get; set; }
        public decimal thn_uw { get; set; }
        public decimal nilai_ttl_ptg { get; set; }
        public decimal nilai_prm { get; set; }
    }

    public class GetGroupAndRekananSorQueryHandler : IRequestHandler<GetGroupAndRekananSorQuery, string>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ILogger<GetGroupAndRekananSorQueryHandler> _logger;

        public GetGroupAndRekananSorQueryHandler(IDbConnectionPst dbConnectionPst,
            ILogger<GetGroupAndRekananSorQueryHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _logger = logger;
        }

        public async Task<string> Handle(GetGroupAndRekananSorQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => (await _dbConnectionPst.QueryProc<string>("spe_ri05e_02",
                new
                {
                    request.kd_jns_sor, request.kd_cob, request.kd_cb,
                    request.thn_uw, request.nilai_ttl_ptg, request.nilai_prm
                })).FirstOrDefault(), _logger);
        }
    }
}