using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaKomisiTambahans.Queries;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.CancelPostingNotaKomisiTambahans.Commands
{
    public class CancelPostingNotaKomisiTambahanCommand : IRequest
    {
        public string DatabaseName { get; set; }

        public List<CancelPostingNotaKomisiTambahanDto> Data { get; set; }
    }

    public class CancelPostingNotaKomisiTambahanCommandHandler : IRequestHandler<CancelPostingNotaKomisiTambahanCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CancelPostingNotaKomisiTambahanCommandHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Unit> Handle(CancelPostingNotaKomisiTambahanCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            foreach (var data in request.Data)
            {
                await _connectionFactory.QueryProc("spp_fn01p_03",
                    new
                    {
                        data.jns_sb_nt, data.kd_cb, data.jns_tr, data.jns_nt_msk, data.kd_thn,
                        data.kd_bln, data.no_nt_msk, data.jns_nt_kel, data.no_nt_kel,
                    });
            }
            
            return Unit.Value;
        }
    }
}