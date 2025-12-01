using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.MutasiKlaims.Commands
{
    public class CheckAlokasiCommand : IRequest<(string, string)>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public string flag_pol_lama { get; set; }

        public string flag_closing { get; set; }
    }

    public class CheckAlokasiCommandHandler : IRequestHandler<CheckAlokasiCommand, (string, string)>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CheckAlokasiCommandHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<(string, string)> Handle(CheckAlokasiCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var result = (await _connectionFactory.QueryProc<(string, string)>("spe_cl02e_13",
                new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn,
                    request.no_kl, request.no_mts, request.flag_pol_lama, request.flag_closing
                })).FirstOrDefault();

            return result;
        }
    }
}