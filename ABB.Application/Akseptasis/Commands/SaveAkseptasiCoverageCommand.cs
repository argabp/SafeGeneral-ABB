using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Commands
{
    public class SaveAkseptasiCoverageCommand : IRequest, IMapFrom<AkseptasiCoverage>
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

        public string kd_cvrg { get; set; }

        public decimal pst_rate_prm { get; set; }

        public byte stn_rate_prm { get; set; }

        public decimal pst_dis { get; set; }

        public decimal pst_kms { get; set; }

        public string flag_pkk { get; set; }

        public string? no_pol_ttg { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveAkseptasiCoverageCommand, AkseptasiCoverage>();
        }
    }

    public class SaveAkseptasiCoverageCommandHandler : IRequestHandler<SaveAkseptasiCoverageCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveAkseptasiCoverageCommandHandler> _logger;

        public SaveAkseptasiCoverageCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory, ILogger<SaveAkseptasiCoverageCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveAkseptasiCoverageCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var result = (await _connectionFactory.QueryProc<string>("spe_uw09e02_02", new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn,
                    request.no_aks, request.no_updt, request.no_rsk, request.kd_endt, 
                    request.pst_rate_prm, request.flag_pkk, request.kd_cvrg
                })).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(result))
                    throw new Exception(result);
                
                var entity = await dbContext.AkseptasiCoverage.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                    request.no_rsk, request.kd_endt, request.kd_cvrg);
            
                if (entity == null)
                {
                    var akseptasiCoverage = _mapper.Map<AkseptasiCoverage>(request);

                    dbContext.AkseptasiCoverage.Add(akseptasiCoverage);

                    await dbContext.SaveChangesAsync(cancellationToken);

                }
                else
                {
                    entity.flag_pkk = request.flag_pkk;
                    entity.pst_dis = request.pst_dis;
                    entity.pst_rate_prm = request.pst_rate_prm;
                    entity.stn_rate_prm = request.stn_rate_prm;
                    entity.pst_kms = request.pst_kms;
            
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                await _connectionFactory.QueryProc<string>("spe_uw02e_20", new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn,
                    request.no_aks, request.no_updt, request.no_rsk, request.kd_endt
                });

                return Unit.Value;
            }, _logger);
        }
    }
}