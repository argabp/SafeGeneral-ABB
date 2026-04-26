using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Akseptasis.Queries;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Common.Queries
{
    public class GetKodeTOLPSTQuery : IRequest<List<DropdownOptionDto>>
    {

        public string kd_cob { get; set; }
    }

    public class GetKodeTOLPSTQueryHandler : IRequestHandler<GetKodeTOLPSTQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<GetKodeTOLPSTQueryHandler> _logger;

        public GetKodeTOLPSTQueryHandler(IDbConnectionPst connectionPst, ILogger<GetKodeTOLPSTQueryHandler> logger)
        {
            _connectionPst = connectionPst;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetKodeTOLPSTQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                return (await _connectionPst.Query<DropdownOptionDto>("SELECT RTRIM(LTRIM(kd_tol)) Value, nm_tol Text " +
                                                                      "FROM rf48 WHERE kd_cob = @kd_cob", new { request.kd_cob })).ToList();
            }, _logger);
        }
    }
}