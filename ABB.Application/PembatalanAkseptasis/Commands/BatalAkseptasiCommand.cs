using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.PembatalanAkseptasis.Commands
{
    public class BatalAkseptasiCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_aks { get; set; }
        public Int16 no_updt { get; set; }
    }

    public class BatalAkseptasiCommandHandler : IRequestHandler<BatalAkseptasiCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public BatalAkseptasiCommandHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Unit> Handle(BatalAkseptasiCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            await _connectionFactory.QueryProc("spp_uw02e_15",
                new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob,
                    request.kd_thn, request.no_aks, request.no_updt
                });

            return Unit.Value;

        }
    }
}