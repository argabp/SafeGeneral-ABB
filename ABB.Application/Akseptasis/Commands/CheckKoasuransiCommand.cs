using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Commands
{
    public class CheckKoasuransiCommand : IRequest<(string, string)>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }
    }

    public class CheckKoasuransiCommandHandler : IRequestHandler<CheckKoasuransiCommand, (string, string)>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CheckKoasuransiCommandHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<(string, string)> Handle(CheckKoasuransiCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var result = (await _connectionFactory.QueryProc<(string, string)>("spe_uw02e_07",
                new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn,
                    request.no_aks, request.no_updt
                })).FirstOrDefault();

            return result;
        }
    }
}