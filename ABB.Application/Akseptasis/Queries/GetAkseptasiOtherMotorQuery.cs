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
    public class GetAkseptasiOtherMotorQuery : IRequest<AkseptasiOtherMotorDto>
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
    }

    public class GetAkseptasiOtherMotorQueryHandler : IRequestHandler<GetAkseptasiOtherMotorQuery, AkseptasiOtherMotorDto?>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAkseptasiOtherMotorQueryHandler> _logger;

        public GetAkseptasiOtherMotorQueryHandler(IDbContextFactory contextFactory, IMapper mapper,
            ILogger<GetAkseptasiOtherMotorQueryHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AkseptasiOtherMotorDto> Handle(GetAkseptasiOtherMotorQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var akseptasiResiko = await dbContext.AkseptasiOtherMotor.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                    request.no_rsk, request.kd_endt);

                return akseptasiResiko == null ? null : _mapper.Map<AkseptasiOtherMotorDto>(akseptasiResiko);
            }, _logger);
        }
    }
}