using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.ReopenMutasiKlaims.Commands
{
    public class ReopenMutasiKlaimCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public Int16 no_mts { get; set; }
    }

    public class ReopenMutasiKlaimCommandHandler : IRequestHandler<ReopenMutasiKlaimCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ReopenMutasiKlaimCommandHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Unit> Handle(ReopenMutasiKlaimCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            await _connectionFactory.QueryProc("spp_cl01p_03",
                new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob,
                    request.kd_thn, request.no_kl, request.no_mts
                });

            return Unit.Value;

        }
    }
}