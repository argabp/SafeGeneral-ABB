using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.EntriNotaKlaimTreaties.Queries
{
    public class GetEntriNotaKlaimTreatyQuery : IRequest<NotaKlaimTreaty>
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

    public class GetEntriNotaKlaimTreatyQueryHandler : IRequestHandler<GetEntriNotaKlaimTreatyQuery, NotaKlaimTreaty>
    {
        private readonly IDbContextPst _dbContextPst;
        private readonly ILogger<GetEntriNotaKlaimTreatyQueryHandler> _logger;

        public GetEntriNotaKlaimTreatyQueryHandler(IDbContextPst dbContextPst,
            ILogger<GetEntriNotaKlaimTreatyQueryHandler> logger)
        {
            _dbContextPst = dbContextPst;
            _logger = logger;
        }

        public async Task<NotaKlaimTreaty> Handle(GetEntriNotaKlaimTreatyQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () => 
            {
                var nota = _dbContextPst.NotaKlaimTreaty.Find(request.kd_cb, request.jns_tr, request.jns_nt_msk,
                    request.kd_thn, request.kd_bln, request.no_nt_msk, request.jns_nt_kel, request.no_nt_kel);
   
                return nota;
            }, _logger);
        }
    }
}