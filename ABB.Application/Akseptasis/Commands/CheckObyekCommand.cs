using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Commands
{
    public class CheckObyekCommand : IRequest<(string, string)>
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public decimal pst_share { get; set; }
    }

    public class CheckObyekCommandHandler : IRequestHandler<CheckObyekCommand, (string, string)>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CheckObyekCommandHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<(string, string)> Handle(CheckObyekCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var result = (await _connectionFactory.QueryProc<(string, string)>("spe_uw02e_53",
                new
                {
                    request.kd_cob, request.kd_scob, request.pst_share
                })).First();

            return result;
        }
    }
}