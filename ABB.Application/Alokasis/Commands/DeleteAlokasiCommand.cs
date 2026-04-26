using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Alokasis.Commands
{
    public class DeleteAlokasiCommand : IRequest
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public Int16 no_updt_reas { get; set; }
    }

    public class DeleteAlokasiCommandHandler : IRequestHandler<DeleteAlokasiCommand>
    {
        private readonly IDbContextPst _dbContextPst;
        private readonly ILogger<GetKodeTOLPSTQueryHandler> _logger;

        public DeleteAlokasiCommandHandler(IDbContextPst dbContextPst,
            ILogger<GetKodeTOLPSTQueryHandler> logger)
        {
            _dbContextPst = dbContextPst;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteAlokasiCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var entity = await _dbContextPst.Alokasi.FindAsync(request.kd_cb,
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_pol, request.no_updt,
                    request.no_rsk, request.kd_endt, request.no_updt_reas);

                if (entity == null)
                    throw new NotFoundException();

                _dbContextPst.Alokasi.Remove(entity);

                await _dbContextPst.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }, _logger);
        }
    }
}