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
    public class GetAkseptasiObyekCITQuery : IRequest<AkseptasiObyekCITDto>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public Int16 no_oby { get; set; }
    }

    public class GetAkseptasiObyekCITQueryHandler : IRequestHandler<GetAkseptasiObyekCITQuery, AkseptasiObyekCITDto>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAkseptasiObyekCITQueryHandler> _logger;

        public GetAkseptasiObyekCITQueryHandler(IDbContextFactory contextFactory, IMapper mapper,
            ILogger<GetAkseptasiObyekCITQueryHandler> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<AkseptasiObyekCITDto> Handle(GetAkseptasiObyekCITQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                    
                var akseptasiResiko = await dbContext.AkseptasiObyekCIT.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                    request.no_rsk, request.kd_endt, request.no_oby);
                                            
                if (akseptasiResiko == null)
                    throw new NullReferenceException();

                return _mapper.Map<AkseptasiObyekCITDto>(akseptasiResiko);
            }, _logger);
        }
    }
}