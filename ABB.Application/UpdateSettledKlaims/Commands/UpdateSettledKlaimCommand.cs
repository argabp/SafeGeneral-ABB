using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Application.UpdateSettledKlaims.Queries;
using MediatR;

namespace ABB.Application.UpdateSettledKlaims.Commands
{
    public class UpdateSettledKlaimCommand : IRequest
    {
        public string DatabaseName { get; set; }

        public List<UpdateSettledKlaimModel> Data { get; set; }
    }

    public class UpdateSettledKlaimCommandHandler : IRequestHandler<UpdateSettledKlaimCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ICurrentUserService _userService;

        public UpdateSettledKlaimCommandHandler(IDbConnectionFactory connectionFactory, ICurrentUserService userService)
        {
            _connectionFactory = connectionFactory;
            _userService = userService;
        }

        public async Task<Unit> Handle(UpdateSettledKlaimCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            foreach (var data in request.Data)
            {
                await _connectionFactory.QueryProc("spp_cl07p_01",
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