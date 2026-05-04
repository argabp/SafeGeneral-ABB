using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.NotaPremiXOLKeluars.Queries
{
    public class GetNotaPremiXOLKeluarQuery : IRequest<NotaKomisiTambahan>
    {
        public string jns_sb_nt { get; set; }

        public string kd_cb { get; set; }

        public string jns_tr { get; set; }
        
        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }
    }

    public class GetNotaPremiXOLKeluarQueryHandler : IRequestHandler<GetNotaPremiXOLKeluarQuery, NotaKomisiTambahan>
    {
        private readonly IDbContextPst _dbContextPst;
        private readonly ILogger<GetNotaPremiXOLKeluarQueryHandler> _logger;

        public GetNotaPremiXOLKeluarQueryHandler(IDbContextPst dbContextPst,
            ILogger<GetNotaPremiXOLKeluarQueryHandler> logger)
        {
            _dbContextPst = dbContextPst;
            _logger = logger;
        }

        public async Task<NotaKomisiTambahan> Handle(GetNotaPremiXOLKeluarQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
            {
                var nota = _dbContextPst.NotaKomisiTambahan.Find(request.jns_sb_nt, request.kd_cb, request.jns_tr, request.jns_nt_msk,
                    request.kd_thn, request.kd_bln, request.no_nt_msk, request.jns_nt_kel, request.no_nt_kel);
   
                return nota;
            }, _logger);
        }
    }
}