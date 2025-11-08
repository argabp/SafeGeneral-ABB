using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.RegisterKlaims.Queries
{
    public class GetPolLamaQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }

        public string flag_tty_msk { get; set; }
    }

    public class GetPolLamaQueryHandler : IRequestHandler<GetPolLamaQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetPolLamaQueryHandler> _logger;

        public GetPolLamaQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetPolLamaQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GetPolLamaQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_cl01e_07", new
                {
                    request.flag_tty_msk
                })).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}