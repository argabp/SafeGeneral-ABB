using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CancelUpdateSettledKlaims.Queries;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using MediatR;

namespace ABB.Application.CancelUpdateSettledKlaims.Commands
{
    public class CancelUpdateSettledKlaimCommand : IRequest
    {
        public string DatabaseName { get; set; }

        public List<CancelUpdateSettledKlaimModel> Data { get; set; }
    }

    public class CancelUpdateSettledKlaimCommandHandler : IRequestHandler<CancelUpdateSettledKlaimCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ICurrentUserService _userService;

        public CancelUpdateSettledKlaimCommandHandler(IDbConnectionFactory connectionFactory, ICurrentUserService userService)
        {
            _connectionFactory = connectionFactory;
            _userService = userService;
        }

        public async Task<Unit> Handle(CancelUpdateSettledKlaimCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            foreach (var data in request.Data)
            {
                await _connectionFactory.QueryProc("spp_cl07p_02",
                    new
                    {
                        data.kd_cb, data.kd_cob, data.kd_scob, data.kd_thn,
                        data.no_kl, tgl_updt = DateTime.Now
                    });
            }
            
            return Unit.Value;
        }
    }
}