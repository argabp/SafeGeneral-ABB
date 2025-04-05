using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Commands
{
    public class CopyEndorsUpdateDeleteCommand : IRequest
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_aks { get; set; }
        public int no_updt { get; set; }
        public int no_rsk { get; set; }
        public string kd_endt { get; set; }
        public string flag_endt { get; set; }
    }

    public class CopyEndorsUpdateDeleteCommandHandler : IRequestHandler<CopyEndorsUpdateDeleteCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CopyEndorsUpdateDeleteCommandHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Unit> Handle(CopyEndorsUpdateDeleteCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            await _connectionFactory.QueryProc("spe_uw02e_18",
                new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob,
                    request.kd_thn, request.no_aks, request.no_updt,
                    request.no_rsk, request.kd_endt, request.flag_endt
                });

            return Unit.Value;

        }
    }
}