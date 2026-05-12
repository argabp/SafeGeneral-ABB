using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Application.PostingNotaTreatyXOLMasuks.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PostingNotaTreatyXOLMasuks.Commands
{
    public class PostingNotaTreatyXOLMasukCommand : IRequest
    {
        public List<PostingNotaTreatyXOLMasukModel> Data { get; set; }
    }

    public class PostingNotaTreatyXOLMasukCommandHandler : IRequestHandler<PostingNotaTreatyXOLMasukCommand>
    {
        private readonly ICurrentUserService _userService;
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ILogger<PostingNotaTreatyXOLMasukCommandHandler> _logger;

        public PostingNotaTreatyXOLMasukCommandHandler(IDbConnectionPst dbConnectionPst, ICurrentUserService userService, 
            ILogger<PostingNotaTreatyXOLMasukCommandHandler> logger)
        {
            _userService = userService;
            _dbConnectionPst = dbConnectionPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(PostingNotaTreatyXOLMasukCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                foreach (var data in request.Data)
                {
                    await _dbConnectionPst.QueryProc("spp_ri01p_02",
                        new
                        {
                            data.kd_cb, data.jns_tr, data.jns_nt_msk,
                            data.kd_thn, data.kd_bln, data.no_nt_msk, data.jns_nt_kel, 
                            data.no_nt_kel, tgl_posting = DateTime.Now, kd_usr_posting = _userService.UserId
                        });
                }
            
                return Unit.Value;
            }, _logger);
        }
    }
}