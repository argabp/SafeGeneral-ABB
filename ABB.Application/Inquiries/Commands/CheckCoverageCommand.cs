using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Inquiries.Commands
{
    public class CheckCoverageCommand : IRequest<(string, string)>
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
    }

    public class CheckCoverageCommandHandler : IRequestHandler<CheckCoverageCommand, (string, string)>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CheckCoverageCommandHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<(string, string)> Handle(CheckCoverageCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var result = (await _connectionFactory.QueryProc<(string, string)>("spe_uw02e_06",
                new
                {
                    request.kd_cob, request.kd_scob
                })).First();

            return result;
        }
    }
}