using System;
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
    public class GetSCOBByKodeCabangPSTQuery : IRequest<List<DropdownOptionDto>>
    {
        public string kd_cob { get; set; }
    }

    public class GetSCOBByKodeCabangPSTQueryHandler : IRequestHandler<GetSCOBByKodeCabangPSTQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<GetSCOBByKodeCabangPSTQueryHandler> _logger;

        public GetSCOBByKodeCabangPSTQueryHandler(IDbConnectionPst connectionPst,
            ILogger<GetSCOBByKodeCabangPSTQueryHandler> logger)
        {
            _connectionPst = connectionPst;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetSCOBByKodeCabangPSTQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                return (await _connectionPst.Query<DropdownOptionDto>(
                    @"SELECT RTRIM(LTRIM(kd_scob)) Value, nm_scob Text FROM rf05 WHERE kd_cob = @kd_cob",
                    new { request.kd_cob })).ToList();
            }, _logger);
        }
    }
}