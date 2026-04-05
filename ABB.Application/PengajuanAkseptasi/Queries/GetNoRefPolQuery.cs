using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PengajuanAkseptasi.Queries
{
    public class GetNoRefPolQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }

        public string jns_pengajuan { get; set; }
    }

    public class GetNoRefPolQueryHandler : IRequestHandler<GetNoRefPolQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetNoRefPolQueryHandler> _logger;

        public GetNoRefPolQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetNoRefPolQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetNoRefPolQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);

                switch (request.jns_pengajuan)
                {
                    case "2":
                    case "3":
                        return (await _connectionFactory.Query<DropdownOptionDto>("SELECT RTRIM(LTRIM(no_pol_ttg)) Value, RTRIM(LTRIM(nm_ttg)) + ' - ' + RTRIM(LTRIM(no_pol_ttg)) Text " +
                            "FROM v_uw01c_endorse")).ToList();
                    case "4":
                        
                        return (await _connectionFactory.Query<DropdownOptionDto>("SELECT RTRIM(LTRIM(no_pol_ttg)) Value, RTRIM(LTRIM(nm_ttg)) + ' - ' + RTRIM(LTRIM(no_pol_ttg)) Text " +
                            "FROM v_uw01c_renewal")).ToList();
                    default:
                        return new List<DropdownOptionDto>();
                }
            }, _logger);
        }
    }
}