using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetAkseptasiPranotaKoasQuery : IRequest<AkseptasiPranotaKoasDto>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public string kd_mtu { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }
    }

    public class GetAkseptasiPranotaKoasQueryHandler : IRequestHandler<GetAkseptasiPranotaKoasQuery, AkseptasiPranotaKoasDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAkseptasiPranotaKoasQueryHandler> _logger;

        public GetAkseptasiPranotaKoasQueryHandler(IDbContextFactory contextFactory, IMapper mapper,
            ILogger<GetAkseptasiPranotaKoasQueryHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AkseptasiPranotaKoasDto> Handle(GetAkseptasiPranotaKoasQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var akseptasiResiko = await dbContext.AkseptasiPranotaKoas.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                    request.kd_mtu, request.kd_grp_pas, request.kd_rk_pas);
                                            
                if (akseptasiResiko == null)
                    throw new NullReferenceException();

                return _mapper.Map<AkseptasiPranotaKoasDto>(akseptasiResiko);
            }, _logger);
        }
    }
}