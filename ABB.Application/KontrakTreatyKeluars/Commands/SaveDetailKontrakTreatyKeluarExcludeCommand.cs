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
    public class SaveDetailKontrakTreatyKeluarExcludeCommand : IRequest, IMapFrom<DetailKontrakTreatyKeluarExclude>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public string kd_okup { get; set; }

        public string? kd_cvrg { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveDetailKontrakTreatyKeluarExcludeCommand, DetailKontrakTreatyKeluarExclude>();
        }
    }

    public class SaveDetailKontrakTreatyKeluarExcludeCommandHandler : IRequestHandler<SaveDetailKontrakTreatyKeluarExcludeCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<SaveDetailKontrakTreatyKeluarExcludeCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SaveDetailKontrakTreatyKeluarExcludeCommandHandler(IDbContextPst contextPst,
            ILogger<SaveDetailKontrakTreatyKeluarExcludeCommandHandler> logger, IMapper mapper)
        {;
            _contextPst = contextPst;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(SaveDetailKontrakTreatyKeluarExcludeCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var detailKontrakTreatyKeluarExclude =
                    _contextPst.DetailKontrakTreatyKeluarExclude.Find(request.kd_cb, request.kd_jns_sor,
                        request.kd_tty_pps, request.kd_okup);
                
                if (detailKontrakTreatyKeluarExclude == null)
                {
                    var newDetailKontrakTreatyKeluarExclude = _mapper.Map<DetailKontrakTreatyKeluarExclude>(request);
                    _contextPst.DetailKontrakTreatyKeluarExclude.Add(newDetailKontrakTreatyKeluarExclude);
                }
                
                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}