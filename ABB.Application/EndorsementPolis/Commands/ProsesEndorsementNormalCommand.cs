using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.EndorsementPolis.Commands
{
    public class ProsesEndorsementNormalCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_pol { get; set; }
        public Int16 no_updt { get; set; }
    }

    public class ProsesEndorsementNormalCommandHandler : IRequestHandler<ProsesEndorsementNormalCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ProsesEndorsementNormalCommandHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Unit> Handle(ProsesEndorsementNormalCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            await _connectionFactory.QueryProc("spp_uw02e_06",
                new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob,
                    request.kd_thn, request.no_pol, request.no_updt
                });

            return Unit.Value;

        }
    }
}