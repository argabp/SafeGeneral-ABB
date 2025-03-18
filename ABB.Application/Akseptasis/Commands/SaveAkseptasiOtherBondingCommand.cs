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
    public class SaveAkseptasiOtherBondingCommand : IRequest, IMapFrom<AkseptasiOtherBonding>
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

        public string kd_grp_prc { get; set; }

        public string? kd_rk_prc { get; set; }

        public string grp_obl { get; set; }

        public string kd_obl { get; set; }

        public string grp_kontr { get; set; }

        public string kd_kontr { get; set; }

        public decimal nilai_bond { get; set; }

        public string nm_obl { get; set; }

        public string? almt_obl { get; set; }

        public string? kt_obl { get; set; }

        public string kd_rumus { get; set; }

        public string nm_kons { get; set; }

        public decimal? nilai_kontr { get; set; }

        public string? no_kontr { get; set; }

        public string? ket_rincian_kontr { get; set; }

        public DateTime? tgl_terbit { get; set; }

        public DateTime? tgl_lelang { get; set; }

        public DateTime? tgl_tr { get; set; }

        public string? ket_pjs { get; set; }

        public string? no_pol_ttg { get; set; }

        public string? nm_principal { get; set; }

        public string? jbt_principal { get; set; }

        public DateTime? tgl_kontrak { get; set; }

        public string? tmpt_lelang { get; set; }

        public string? ba_serah_trm { get; set; }

        public string grp_jns_pekerjaan { get; set; }

        public string jns_pekerjaan { get; set; }

        public string? kd_rk_obl { get; set; }

        public string kd_grp_obl { get; set; }

        public string kd_grp_surety { get; set; }

        public string kd_rk_surety { get; set; }

        public string kd_bag { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveAkseptasiOtherBondingCommand, AkseptasiOtherBonding>();
        }
    }

    public class SaveAkseptasiOtherBondingCommandHandler : IRequestHandler<SaveAkseptasiOtherBondingCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveAkseptasiOtherBondingCommandHandler> _logger;

        public SaveAkseptasiOtherBondingCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory, ILogger<SaveAkseptasiOtherBondingCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveAkseptasiOtherBondingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var entity = await dbContext.AkseptasiOtherBonding.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                    request.no_rsk, request.kd_endt);
            
                if (entity == null)
                {
                    var akseptasiOther = _mapper.Map<AkseptasiOtherBonding>(request);

                    dbContext.AkseptasiOtherBonding.Add(akseptasiOther);

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