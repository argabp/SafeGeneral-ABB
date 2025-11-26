using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CancelPostingMutasiKlaims.Queries;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using MediatR;

namespace ABB.Application.CancelPostingMutasiKlaims.Commands
{
    public class PostingAccountingCommand : IRequest
    {
        public string DatabaseName { get; set; }

        public List<CancelPostingMutasiKlaimModel> Data { get; set; }
    }

    public class PostingAccountingCommandHandler : IRequestHandler<PostingAccountingCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ICurrentUserService _userService;

        public PostingAccountingCommandHandler(IDbConnectionFactory connectionFactory, ICurrentUserService userService)
        {
            _connectionFactory = connectionFactory;
            _userService = userService;
        }

        public async Task<Unit> Handle(PostingAccountingCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            foreach (var data in request.Data)
            {
                await _connectionFactory.QueryProc("spp_cl02p_04",
                    new
                    {
                        data.kd_cb, data.kd_cob, data.kd_scob, data.kd_thn,
                        data.no_kl, data.no_mts, tgl_posting = DateTime.Now,
                        kd_usr_posting = _userService.UserId
                    });
            }
            
            return Unit.Value;
        }
    }
}