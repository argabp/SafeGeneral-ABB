using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.KontrakTreatyKeluars.Queries;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KontrakTreatyKeluars.Commands
{
    public class SaveDetailKontrakTreatyKeluarSCOBCommand : IRequest, IMapFrom<DetailKontrakTreatyKeluarSCOB>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public string kd_cob { get; set; }
        
        public string kd_scob { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveDetailKontrakTreatyKeluarSCOBCommand, DetailKontrakTreatyKeluarSCOB>();
        }
    }

    public class SaveDetailKontrakTreatyKeluarSCOBCommandHandler : IRequestHandler<SaveDetailKontrakTreatyKeluarSCOBCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<SaveDetailKontrakTreatyKeluarSCOBCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SaveDetailKontrakTreatyKeluarSCOBCommandHandler(IDbContextPst contextPst,
            ILogger<SaveDetailKontrakTreatyKeluarSCOBCommandHandler> logger, IMapper mapper)
        {;
            _contextPst = contextPst;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(SaveDetailKontrakTreatyKeluarSCOBCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var detailKontrakTreatyKeluarSCOB =
                    _contextPst.DetailKontrakTreatyKeluarSCOB.Find(request.kd_cb, request.kd_jns_sor,
                        request.kd_tty_pps, request.kd_cob, request.kd_scob);
                
                if (detailKontrakTreatyKeluarSCOB == null)
                {
                    var newDetailKontrakTreatyKeluarSCOB = _mapper.Map<DetailKontrakTreatyKeluarSCOB>(request);
                    _contextPst.DetailKontrakTreatyKeluarSCOB.Add(newDetailKontrakTreatyKeluarSCOB);
                }
                
                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}