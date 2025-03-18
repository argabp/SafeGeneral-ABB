using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.RenewalPolis.Commands
{
    public class RenewalPolisCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_pol { get; set; }
        public Int16 no_updt { get; set; }
        public string kd_scob_new { get; set; }
    }

    public class RenewalPolisCommandHandler : IRequestHandler<RenewalPolisCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public RenewalPolisCommandHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Unit> Handle(RenewalPolisCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            await _connectionFactory.QueryProc("spp_uw02e_09",
                new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob,
                    request.kd_thn, request.no_pol, request.no_updt,
                    request.kd_scob_new
                });

            return Unit.Value;

        }
    }
}