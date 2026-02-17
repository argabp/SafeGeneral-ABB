using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetAkseptasiPranotaQuery : IRequest<AkseptasiPranotaDto>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public string kd_mtu { get; set; }
    }

    public class GetAkseptasiPranotaQueryHandler : IRequestHandler<GetAkseptasiPranotaQuery, AkseptasiPranotaDto?>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAkseptasiPranotaQueryHandler> _logger;

        public GetAkseptasiPranotaQueryHandler(IDbContextFactory contextFactory, IMapper mapper,
            ILogger<GetAkseptasiPranotaQueryHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AkseptasiPranotaDto> Handle(GetAkseptasiPranotaQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                    
                var akseptasiResiko = dbContext.AkseptasiPranota.FirstOrDefault(w => 
                        w.kd_cb == request.kd_cb && w.kd_cob == request.kd_cob &&
                        w.kd_scob == request.kd_scob && w.kd_thn == request.kd_thn &&
                        w.no_aks == request.no_aks && w.no_updt == request.no_updt);

                return akseptasiResiko == null ? null : _mapper.Map<AkseptasiPranotaDto>(akseptasiResiko);
            }, _logger);
        }
    }
}