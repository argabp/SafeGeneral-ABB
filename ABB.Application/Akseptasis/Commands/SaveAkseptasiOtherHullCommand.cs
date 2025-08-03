using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Commands
{
    public class SaveAkseptasiOtherHullCommand : IRequest, IMapFrom<AkseptasiOtherHull>
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

        public string kd_kapal { get; set; }

        public string? nm_kapal { get; set; }

        public string? merk_kapal { get; set; }

        public string no_rangka { get; set; }

        public string no_msn { get; set; }

        public decimal? thn_buat { get; set; }

        public int? grt { get; set; }

        public string? no_pol_ttg { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveAkseptasiOtherHullCommand, AkseptasiOtherHull>();
        }
    }

    public class SaveAkseptasiOtherHullCommandHandler : IRequestHandler<SaveAkseptasiOtherHullCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveAkseptasiOtherHullCommandHandler> _logger;

        public SaveAkseptasiOtherHullCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory, ILogger<SaveAkseptasiOtherHullCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveAkseptasiOtherHullCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var entity = await dbContext.AkseptasiOtherHull.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                    request.no_rsk, request.kd_endt);
            
                if (entity == null)
                {
                    var akseptasiOther = _mapper.Map<AkseptasiOtherHull>(request);

                    dbContext.AkseptasiOtherHull.Add(akseptasiOther);

                    await dbContext.SaveChangesAsync(cancellationToken);

                }
                else
                {
                    entity.kd_kapal = request.kd_kapal;
                    entity.nm_kapal = request.nm_kapal;
                    entity.merk_kapal = request.merk_kapal;
                    entity.no_rangka = request.no_rangka;
                    entity.no_msn = request.no_msn;
                    entity.thn_buat = request.thn_buat;
                    entity.grt = request.grt;
            
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                await _connectionFactory.QueryProc<string>("spe_uw02e_20", new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob,
                    request.kd_thn, request.no_aks, request.no_updt,
                    request.no_rsk, request.kd_endt
                });

                return Unit.Value;
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException == null ? e.Message : e.InnerException.Message);
                throw e.InnerException ?? e;
            }
        }
    }
}