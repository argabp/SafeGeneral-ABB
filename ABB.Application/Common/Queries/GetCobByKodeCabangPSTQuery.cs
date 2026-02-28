using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Common.Queries
{
    public class GetCobByKodeCabangPSTQuery : IRequest<List<DropdownOptionDto>>
    {
    }

    public class GetCobByKodeCabangPSTQueryHandler : IRequestHandler<GetCobByKodeCabangPSTQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<GetCobByKodeCabangPSTQueryHandler> _logger;

        public GetCobByKodeCabangPSTQueryHandler(IDbConnectionPst connectionPst, 
            ILogger<GetCobByKodeCabangPSTQueryHandler> logger)
        {
            _connectionPst = connectionPst;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetCobByKodeCabangPSTQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
                (await _connectionPst.Query<DropdownOptionDto>("SELECT RTRIM(LTRIM(kd_cob)) Value, nm_cob Text " +
                    "FROM rf04")).ToList(), _logger);
        }
    }
}