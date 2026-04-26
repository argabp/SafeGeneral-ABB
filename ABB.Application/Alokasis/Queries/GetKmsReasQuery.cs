using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Alokasis.Queries
{
    public class GetKmsReasQuery : IRequest<string>
    {
        public decimal pst_kms_reas { get; set; }
        public decimal nilai_prm_reas { get; set; }
        public decimal nilai_adj_reas { get; set; }
    }

    public class GetKmsReasQueryHandler : IRequestHandler<GetKmsReasQuery, string>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ILogger<GetKmsReasQueryHandler> _logger;

        public GetKmsReasQueryHandler(IDbConnectionPst dbConnectionPst,
            ILogger<GetKmsReasQueryHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _logger = logger;
        }

        public async Task<string> Handle(GetKmsReasQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => (await _dbConnectionPst.QueryProc<string>("spe_ri05e_04",
                new
                {
                    request.pst_kms_reas, request.nilai_prm_reas, request.nilai_adj_reas
                })).FirstOrDefault(), _logger);
        }
    }
}