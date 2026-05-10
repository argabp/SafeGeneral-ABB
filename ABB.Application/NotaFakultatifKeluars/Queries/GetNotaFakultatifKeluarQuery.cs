using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.NotaFakultatifKeluars.Queries
{
    public class GetNotaFakultatifKeluarQuery : IRequest<NotaFakultatifKeluar>
    {
        public string kd_cb { get; set; }

        public string jns_tr { get; set; }

        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }
    }

    public class GetNotaFakultatifKeluarQueryHandler : IRequestHandler<GetNotaFakultatifKeluarQuery, NotaFakultatifKeluar>
    {
        private readonly IDbContextPst _dbContextPst;
        private readonly ILogger<GetNotaFakultatifKeluarQuery> _logger;

        public GetNotaFakultatifKeluarQueryHandler(IDbContextPst dbContextPst,
            ILogger<GetNotaFakultatifKeluarQuery> logger)
        {
            _dbContextPst = dbContextPst;
            _logger = logger;
        }

        public async Task<NotaFakultatifKeluar> Handle(GetNotaFakultatifKeluarQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(() =>
            {
                var notaFakultatifKeluar =
                    _dbContextPst.NotaFakultatifKeluar.Find(request.kd_cb, request.jns_tr, request.jns_nt_msk,
                        request.kd_thn, request.kd_bln, request.no_nt_msk, request.jns_nt_kel, request.no_nt_kel);

                return Task.FromResult(notaFakultatifKeluar);
            }, _logger);
        }
    }
}