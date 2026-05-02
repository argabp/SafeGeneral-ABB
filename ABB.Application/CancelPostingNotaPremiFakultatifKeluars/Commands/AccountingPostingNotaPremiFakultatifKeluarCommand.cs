using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaPremiFakultatifKeluars.Queries;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelPostingNotaPremiFakultatifKeluars.Commands
{
    public class AccountingPostingNotaPremiFakultatifKeluarCommand : IRequest
    {
        public List<AccountingPostingNotaPremiFakultatifKeluarModel> Data { get; set; }
    }

    public class AccountingPostingNotaPremiFakultatifKeluarCommandHandler : IRequestHandler<AccountingPostingNotaPremiFakultatifKeluarCommand>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ICurrentUserService _userService;
        private readonly ILogger<AccountingPostingNotaPremiFakultatifKeluarCommandHandler> _logger;

        public AccountingPostingNotaPremiFakultatifKeluarCommandHandler(IDbConnectionPst dbConnectionPst, ICurrentUserService userService,
            ILogger<AccountingPostingNotaPremiFakultatifKeluarCommandHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _userService = userService;
            _logger = logger;
        }

        public async Task<Unit> Handle(AccountingPostingNotaPremiFakultatifKeluarCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                foreach (var data in request.Data)
                {
                    await _dbConnectionPst.QueryProc("spp_ri03p_04",
                        new
                        {
                            data.kd_cb, data.jns_tr, data.jns_nt_msk,
                            data.kd_thn, data.kd_bln, data.no_nt_msk,
                            data.jns_nt_kel, data.no_nt_kel,
                            tgl_posting = DateTime.Now, kd_usr_posting = _userService.UserId
                        });
                }
            
                return Unit.Value;
            }, _logger);
        }
    }
}