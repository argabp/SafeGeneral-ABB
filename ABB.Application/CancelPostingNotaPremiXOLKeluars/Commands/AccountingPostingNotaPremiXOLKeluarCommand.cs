using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaPremiXOLKeluars.Queries;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelPostingNotaPremiXOLKeluars.Commands
{
    public class AccountingPostingNotaPremiXOLKeluarCommand : IRequest
    {
        public List<AccountingPostingNotaPremiXOLKeluarModel> Data { get; set; }
    }

    public class AccountingPostingNotaPremiXOLKeluarCommandHandler : IRequestHandler<AccountingPostingNotaPremiXOLKeluarCommand>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ICurrentUserService _userService;
        private readonly ILogger<AccountingPostingNotaPremiXOLKeluarCommandHandler> _logger;

        public AccountingPostingNotaPremiXOLKeluarCommandHandler(IDbConnectionPst dbConnectionPst, ICurrentUserService userService,
            ILogger<AccountingPostingNotaPremiXOLKeluarCommandHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _userService = userService;
            _logger = logger;
        }

        public async Task<Unit> Handle(AccountingPostingNotaPremiXOLKeluarCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                foreach (var data in request.Data)
                {
                    await _dbConnectionPst.QueryProc("spp_fn01p_04",
                        new
                        {
                            data.jns_sb_nt,
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