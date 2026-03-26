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
    public class SaveDetailKontrakTreatyKeluarCoverageCommand : IRequest, IMapFrom<DetailKontrakTreatyKeluarCoverage>
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public string kd_cvrg { get; set; }

        public decimal pst_kms_reas { get; set; }

        public decimal? max_limit_jktb { get; set; }
        
        public decimal? max_limit_prov { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveDetailKontrakTreatyKeluarCoverageCommand, DetailKontrakTreatyKeluarCoverage>();
        }
    }

    public class SaveDetailKontrakTreatyKeluarCoverageCommandHandler : IRequestHandler<SaveDetailKontrakTreatyKeluarCoverageCommand>
    {
        private readonly IDbContextPst _contextPst;
        private readonly ILogger<SaveDetailKontrakTreatyKeluarCoverageCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SaveDetailKontrakTreatyKeluarCoverageCommandHandler(IDbContextPst contextPst,
            ILogger<SaveDetailKontrakTreatyKeluarCoverageCommandHandler> logger, IMapper mapper)
        {;
            _contextPst = contextPst;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(SaveDetailKontrakTreatyKeluarCoverageCommand request, CancellationToken cancellationToken)
        {
            await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var detailKontrakTreatyKeluarCoverage =
                    _contextPst.DetailKontrakTreatyKeluarCoverage.Find(request.kd_cb, request.kd_jns_sor,
                        request.kd_tty_pps, request.kd_cvrg);
                
                if (detailKontrakTreatyKeluarCoverage == null)
                {
                    var newDetailKontrakTreatyKeluarCoverage = _mapper.Map<DetailKontrakTreatyKeluarCoverage>(request);
                    _contextPst.DetailKontrakTreatyKeluarCoverage.Add(newDetailKontrakTreatyKeluarCoverage);
                }
                else
                {
                    detailKontrakTreatyKeluarCoverage.pst_kms_reas = request.pst_kms_reas;
                    detailKontrakTreatyKeluarCoverage.max_limit_jktb = request.max_limit_jktb;
                    detailKontrakTreatyKeluarCoverage.max_limit_prov = request.max_limit_prov;
                }
                
                await _contextPst.SaveChangesAsync(cancellationToken);
            }, _logger);

            return Unit.Value;
        }
    }
}