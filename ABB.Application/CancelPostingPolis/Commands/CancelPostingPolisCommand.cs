using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaKomisiTambahans.Queries;
using ABB.Application.CancelPostingPolis.Queries;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.CancelPostingPolis.Commands
{
    public class CancelPostingPolisCommand : IRequest
    {
        public string DatabaseName { get; set; }

        public List<CancelPostingPolisDto> Data { get; set; }
    }

    public class CancelPostingPolisCommandHandler : IRequestHandler<CancelPostingPolisCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CancelPostingPolisCommandHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Unit> Handle(CancelPostingPolisCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            foreach (var data in request.Data)
            {
                await _connectionFactory.QueryProc("spp_uw02p_03",
                    new
                    {
                        data.kd_cb, data.kd_cob, data.kd_scob,
                        data.kd_thn, data.no_pol, data.no_updt
                    });
            }
            
            return Unit.Value;
        }
    }
}