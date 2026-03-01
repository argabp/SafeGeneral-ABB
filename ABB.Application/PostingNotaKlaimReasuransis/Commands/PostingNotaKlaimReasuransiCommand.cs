using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Application.PostingNotaKlaimReasuransis.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PostingNotaKlaimReasuransis.Commands
{
    public class PostingNotaKlaimReasuransiCommand : IRequest
    {
        public List<PostingNotaKlaimReasuransiModel> Data { get; set; }
    }

    public class PostingNotaKlaimReasuransiCommandHandler : IRequestHandler<PostingNotaKlaimReasuransiCommand>
    {
        private readonly IDbConnectionPst _dbConnectionPst;
        private readonly ICurrentUserService _userService;
        private readonly ILogger<PostingNotaKlaimReasuransiCommandHandler> _logger;

        public PostingNotaKlaimReasuransiCommandHandler(IDbConnectionPst dbConnectionPst, ICurrentUserService userService,
            ILogger<PostingNotaKlaimReasuransiCommandHandler> logger)
        {
            _dbConnectionPst = dbConnectionPst;
            _userService = userService;
            _logger = logger;
        }

        public async Task<Unit> Handle(PostingNotaKlaimReasuransiCommand request, CancellationToken lationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                foreach (var data in request.Data)
                {
                    await _dbConnectionPst.QueryProc("spp_cl05p_02",
                        new
                        {
                            data.kd_cb, data.kd_cob, data.kd_scob,
                            data.kd_thn, data.no_kl, data.no_mts,
                            tgl_posting = DateTime.Now, kd_usr_posting = _userService.UserId
                        });
                }
            
                return Unit.Value;
            }, _logger);
        }
    }
}