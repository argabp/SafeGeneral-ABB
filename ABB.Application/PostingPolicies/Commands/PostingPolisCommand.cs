using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Application.PostingPolicies.Queries;
using MediatR;

namespace ABB.Application.PostingPolicies.Commands
{
    public class PostingPolisCommand : IRequest
    {
        public string DatabaseName { get; set; }

        public List<PostingPolisDto> Data { get; set; }
    }

    public class PostingPolisCommandHandler : IRequestHandler<PostingPolisCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ICurrentUserService _userService;

        public PostingPolisCommandHandler(IDbConnectionFactory connectionFactory, ICurrentUserService userService)
        {
            _connectionFactory = connectionFactory;
            _userService = userService;
        }

        public async Task<Unit> Handle(PostingPolisCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            foreach (var data in request.Data)
            {
                await _connectionFactory.QueryProc("spp_uw02p_02",
                    new
                    {
                        data.kd_cb, data.kd_cob, data.kd_scob, data.kd_thn,
                        data.no_pol, data.no_updt, data.no_pol_ttg, 
                        tgl_posting = DateTime.Now, kd_usr_posting = _userService.UserName
                    });
            }
            
            return Unit.Value;
        }
    }
}