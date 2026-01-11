using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.MutasiKlaims.Commands
{
    public class ClosingMutasiKlaimCommand : IRequest<(string, string, string)>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public Int16 no_mts { get; set; }

        public DateTime? tgl_closing { get; set; }

        public string kd_usr_input { get; set; }
    }

    public class ClosingMutasiKlaimCommandHandler : IRequestHandler<ClosingMutasiKlaimCommand, (string, string, string)>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ClosingMutasiKlaimCommandHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<(string, string, string)> Handle(ClosingMutasiKlaimCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<(string, string, string)>("spp_cl01p_02",
                new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob,
                    request.kd_thn, request.no_kl, request.no_mts,
                    request.tgl_closing, request.kd_usr_input, kd_pass = string.Empty
                })).FirstOrDefault();
        }
    }
}