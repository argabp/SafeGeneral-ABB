using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Inquiries.Commands
{
    public class CheckOtherCommand : IRequest<(string, string)>
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public decimal pst_share { get; set; }
    }

    public class CheckOtherCommandHandler : IRequestHandler<CheckOtherCommand, (string, string)>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CheckOtherCommandHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<(string, string)> Handle(CheckOtherCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var result = (await _connectionFactory.QueryProc<(string, string)>("spe_uw02e_04",
                new
                {
                    request.kd_cob, request.kd_scob, request.pst_share
                })).First();

            return result;
        }
    }
}