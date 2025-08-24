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
    public class SaveAkseptasiPranotaKoasCommand : IRequest, IMapFrom<AkseptasiPranotaKoas>
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

        public decimal? pst_share { get; set; }

        public string? kd_prm { get; set; }

        public decimal? nilai_prm { get; set; }

        public decimal? pst_dis { get; set; }

        public decimal? nilai_dis { get; set; }

        public decimal? pst_kms { get; set; }

        public decimal? nilai_kms { get; set; }

        public decimal? pst_hf { get; set; }

        public decimal? nilai_hf { get; set; }

        public decimal? nilai_kl { get; set; }

        public decimal? pst_pjk { get; set; }

        public decimal? nilai_pjk { get; set; }

        public string? no_pol_ttg { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveAkseptasiPranotaKoasCommand, AkseptasiPranotaKoas>();
        }
    }

    public class SaveAkseptasiPranotaKoasCommandHandler : IRequestHandler<SaveAkseptasiPranotaKoasCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveAkseptasiPranotaKoasCommandHandler> _logger;

        public SaveAkseptasiPranotaKoasCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory, ILogger<SaveAkseptasiPranotaKoasCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveAkseptasiPranotaKoasCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var entity = await dbContext.AkseptasiPranotaKoas.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                    request.kd_mtu, request.kd_grp_pas, request.kd_rk_pas);
            
                if (entity == null)
                {
                    var akseptasiPranotaKoas = _mapper.Map<AkseptasiPranotaKoas>(request);

                    akseptasiPranotaKoas.kd_prm = "PRM";
                    
                    dbContext.AkseptasiPranotaKoas.Add(akseptasiPranotaKoas);

                    await dbContext.SaveChangesAsync(cancellationToken);

                }
                else
                {
                    entity.pst_share = request.pst_share;
                    entity.nilai_prm = request.nilai_prm;
                    entity.pst_kms = request.pst_kms;
                    entity.nilai_kms = request.nilai_kms;
                    entity.pst_dis = request.pst_dis;
                    entity.nilai_dis = request.nilai_dis;;
                    entity.pst_hf = request.pst_hf;
                    entity.nilai_hf = request.nilai_hf;
            
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

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