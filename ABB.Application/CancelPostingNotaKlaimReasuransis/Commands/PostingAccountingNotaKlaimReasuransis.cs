using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.CancelPostingNotaKlaimReasuransis.Queries;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.CancelPostingNotaKlaimReasuransis.Commands
{
    public class PostingAccountingNotaKlaimReasuransis : IRequest
    {

        public List<CancelPostingNotaKlaimReasuransiModel> Data { get; set; }
    }

    public class PostingAccountingNotaKlaimReasuransisHandler : IRequestHandler<PostingAccountingNotaKlaimReasuransis>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ICurrentUserService _userService;
        private readonly ILogger<PostingAccountingNotaKlaimReasuransisHandler> _logger;

        public PostingAccountingNotaKlaimReasuransisHandler(IDbConnectionFactory connectionFactory, 
            ICurrentUserService userService, ILogger<PostingAccountingNotaKlaimReasuransisHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _userService = userService;
            _logger = logger;
        }

        public async Task<Unit> Handle(PostingAccountingNotaKlaimReasuransis request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                foreach (var data in request.Data)
                {
                    await _connectionFactory.QueryProc("spp_cl05p_04",
                        new
                        {
                            data.kd_cb, data.kd_cob, data.kd_scob, data.kd_thn,
                            data.no_kl, data.no_mts, tgl_posting = DateTime.Now,
                            kd_usr_posting = _userService.UserId
                        });
                }

                return Unit.Value;
            }, _logger);
        }
    }
}