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
    public class GetRekananSorPSTQuery : IRequest<List<DropdownOptionDto>>
    {
        public string jns_lookup { get; set; }
    }

    public class GetRekananSorPSTQueryHandler : IRequestHandler<GetRekananSorPSTQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<GetRekananSorPSTQueryHandler> _logger;

        public GetRekananSorPSTQueryHandler(IDbConnectionPst connectionPst,
            ILogger<GetRekananSorPSTQueryHandler> logger)
        {
            _connectionPst = connectionPst;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetRekananSorPSTQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var result = (await _connectionPst.QueryProc<string>("spg_ddlb_all",
                    new
                    {
                        request.jns_lookup, nm_kolom = "kd_rk_sor"
                    })).FirstOrDefault();

                var data = result?.Split("#")[1];

                var menuName = data?.Split(",")[0];
                var dbName = data?.Split(",")[1];

                List<DropdownOptionDto> dropdown;

                if (menuName.Contains("Rekanan"))
                {
                    dropdown = (await _connectionPst.Query<DropdownOptionDto>(
                        $"SELECT kd_rk Value, nm_rk Text FROM {dbName} WHERE kd_grp_rk = '5'")).ToList();
                }
                else
                {
                    dropdown = (await _connectionPst.Query<DropdownOptionDto>(
                        $"SELECT kd_tty_pps Value, nm_tty_pps Text FROM {dbName}")).ToList();
                }

                return dropdown;
            }, _logger);
        }
    }
}