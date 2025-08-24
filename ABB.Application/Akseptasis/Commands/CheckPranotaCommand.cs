using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Commands
{
    public class CheckPranotaCommand : IRequest<(string, string)>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }
    }

    public class CheckPranotaCommandHandler : IRequestHandler<CheckPranotaCommand, (string, string)>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CheckPranotaCommandHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<(string, string)> Handle(CheckPranotaCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var result = (await _connectionFactory.QueryProc<(string, string)>("spe_uw02e_08",
                new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn,
                    request.no_aks, request.no_updt
                })).FirstOrDefault();

            return result;
        }
    }
}