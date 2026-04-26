using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Alokasis.Queries
{
    public class GetAlokasiQuery : IRequest<Alokasi>
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

    public class GetAlokasiQueryHandler : IRequestHandler<GetAlokasiQuery, Alokasi>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IDbContextPst _dbContextPst;
        private readonly ILogger<GetAlokasiQueryHandler> _logger;

        public GetAlokasiQueryHandler(IDbContextPst dbContextPst,
            ILogger<GetAlokasiQueryHandler> logger)
        {
            _dbContextPst = dbContextPst;
            _logger = logger;
        }

        public async Task<Alokasi> Handle(GetAlokasiQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var entity = await _dbContextPst.Alokasi.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_pol, request.no_updt, 
                    request.no_rsk, request.kd_endt, request.no_updt_reas);
                                         
                if (entity == null)
                    throw new NullReferenceException("Alokasi tidak dapat ditemukan");

                return entity;
            }, _logger);
        }
    }
}