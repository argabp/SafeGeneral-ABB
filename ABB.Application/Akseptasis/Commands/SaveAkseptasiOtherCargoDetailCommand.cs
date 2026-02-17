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
    public class SaveAkseptasiOtherCargoDetailCommand : IRequest, IMapFrom<AkseptasiOtherCargoDetail>
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

        public Int16 no_urut { get; set; }

        public string jns_angkut { get; set; }

        public string? kd_angkut { get; set; }

        public string nm_angkut { get; set; }

        public string no_bl { get; set; }
        
        public string no_inv { get; set; }
        
        public string no_po { get; set; }
        
        public string? no_pol_ttg { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveAkseptasiOtherCargoDetailCommand, AkseptasiOtherCargoDetail>();
        }
    }

    public class SaveAkseptasiOtherCargoDetailCommandHandler : IRequestHandler<SaveAkseptasiOtherCargoDetailCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveAkseptasiOtherCargoDetailCommandHandler> _logger;

        public SaveAkseptasiOtherCargoDetailCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory, ILogger<SaveAkseptasiOtherCargoDetailCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveAkseptasiOtherCargoDetailCommand request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);

                var no_urut = dbContext.AkseptasiOtherCargoDetail.Count(w =>
                    w.kd_cb == request.kd_cb && w.kd_cob == request.kd_cob &&
                    w.kd_scob == request.kd_scob && w.kd_thn == request.kd_thn &&
                    w.no_aks == request.no_aks && w.no_updt == request.no_updt &&
                    w.no_rsk == request.no_rsk && w.kd_endt == request.kd_endt) + 1;
                
                var entity = await dbContext.AkseptasiOtherCargoDetail.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                    request.no_rsk, request.kd_endt, request.no_urut);
            
                if (entity == null)
                {
                    var akseptasiOtherCargoDetail = _mapper.Map<AkseptasiOtherCargoDetail>(request);

                    akseptasiOtherCargoDetail.no_urut = (short) no_urut;
                    
                    dbContext.AkseptasiOtherCargoDetail.Add(akseptasiOtherCargoDetail);

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    entity.jns_angkut = request.jns_angkut;
                    entity.kd_angkut = request.kd_angkut;
                    entity.nm_angkut = request.nm_angkut;
                    entity.no_bl = request.no_bl;
                    entity.no_po = request.no_po;
                    entity.no_inv = request.no_inv;
            
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                return Unit.Value;
            }, _logger);
        }
    }
}