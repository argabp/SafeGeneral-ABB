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
    public class SaveAkseptasiPranotaCommand : IRequest, IMapFrom<AkseptasiPranota>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public string kd_mtu { get; set; }

        public string? kd_prm { get; set; }

        public decimal? nilai_prm { get; set; }

        public decimal? pst_dis { get; set; }

        public decimal? nilai_dis { get; set; }

        public decimal? pst_dis_fea { get; set; }

        public decimal? nilai_dis_fea { get; set; }

        public decimal? pst_dis_fleet { get; set; }

        public decimal? nilai_dis_fleet { get; set; }

        public decimal? nilai_insentif { get; set; }

        public decimal? nilai_bia_pol { get; set; }

        public decimal? nilai_bia_mat { get; set; }

        public decimal? pst_kms { get; set; }

        public decimal? nilai_kms { get; set; }

        public decimal? pst_hf { get; set; }

        public decimal? nilai_hf { get; set; }

        public decimal? pst_kms_reas { get; set; }

        public decimal? nilai_kms_reas { get; set; }

        public decimal? nilai_bia_supl { get; set; }

        public decimal? nilai_bia_pu { get; set; }

        public decimal? nilai_bia_pbtl { get; set; }

        public decimal? nilai_bia_form { get; set; }

        public decimal? nilai_kl { get; set; }

        public decimal? pst_pjk { get; set; }

        public decimal? nilai_pjk { get; set; }

        public decimal? nilai_ttl_kms { get; set; }

        public decimal? nilai_ttl_bia { get; set; }

        public decimal? nilai_ttl_ptg { get; set; }

        public string? no_pol_ttg { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveAkseptasiPranotaCommand, AkseptasiPranota>();
        }
    }

    public class SaveAkseptasiPranotaCommandHandler : IRequestHandler<SaveAkseptasiPranotaCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveAkseptasiPranotaCommandHandler> _logger;

        public SaveAkseptasiPranotaCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory, ILogger<SaveAkseptasiPranotaCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveAkseptasiPranotaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var entity = await dbContext.AkseptasiPranota.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                    request.kd_mtu);
            
                if (entity == null)
                {
                    var akseptasiPranota = _mapper.Map<AkseptasiPranota>(request);

                    dbContext.AkseptasiPranota.Add(akseptasiPranota);

                    await dbContext.SaveChangesAsync(cancellationToken);

                }
                else
                {
                    _mapper.Map(request, entity);

                    if(entity.kd_cb.Length != 5)
                        for (int sequence = entity.kd_cb.Length; sequence < 5; sequence++)
                        {
                            entity.kd_cb += " ";
                        }
            
                    if(entity.kd_cob.Length != 2)
                        for (int sequence = entity.kd_cob.Length; sequence < 2; sequence++)
                        {
                            entity.kd_cob += " ";
                        }

                    if(entity.kd_scob.Length != 5)
                        for (int sequence = entity.kd_scob.Length; sequence < 5; sequence++)
                        {
                            entity.kd_scob += " ";
                        }
            
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