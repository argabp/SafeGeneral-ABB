using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Application.PostingMutasiKlaims.Queries;
using MediatR;

namespace ABB.Application.PostingMutasiKlaims.Commands
{
    public class PostingMutasiKlaimCommand : IRequest
    {
        public string DatabaseName { get; set; }

        public List<PostingMutasiKlaimModel> Data { get; set; }
    }

    public class PostingMutasiKlaimCommandHandler : IRequestHandler<PostingMutasiKlaimCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ICurrentUserService _userService;

        public PostingMutasiKlaimCommandHandler(IDbConnectionFactory connectionFactory, ICurrentUserService userService)
        {
            _connectionFactory = connectionFactory;
            _userService = userService;
        }

        public async Task<Unit> Handle(PostingMutasiKlaimCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            foreach (var data in request.Data)
            {
                await _connectionFactory.QueryProc("spp_cl02p_02",
                    new
                    {
                        data.kd_cb, data.kd_cob, data.kd_scob, data.kd_thn,
                        data.no_kl, data.no_mts, tgl_posting = DateTime.Now, kd_usr_posting = _userService.UserName
                    });
            }
            
            return Unit.Value;
        }
    }
}