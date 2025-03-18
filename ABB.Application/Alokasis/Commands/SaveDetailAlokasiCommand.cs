using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Alokasis.Commands
{
    public class SaveDetailAlokasiCommand : IRequest, IMapFrom<DetailAlokasi>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public Int16 no_updt_reas { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_grp_sor { get; set; }

        public string kd_rk_sor { get; set; }

        public decimal pst_share { get; set; }

        public decimal nilai_prm_reas { get; set; }

        public decimal pst_adj_reas { get; set; }

        public byte stn_adj_reas { get; set; }

        public decimal nilai_adj_reas { get; set; }

        public decimal pst_kms_reas { get; set; }

        public decimal nilai_kms_reas { get; set; }

        public decimal pst_pjk_reas { get; set; }

        public decimal nilai_pjk_reas { get; set; }

        public decimal nilai_ttl_ptg_reas { get; set; }

        public string? no_pol_ttg { get; set; }

        public string kd_grp_sb_bis { get; set; }
        public string kd_rk_sb_bis { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveDetailAlokasiCommand, DetailAlokasi>();
        }
    }

    public class SaveDetailAlokasiCommandHandler : IRequestHandler<SaveDetailAlokasiCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveDetailAlokasiCommandHandler> _logger;

        public SaveDetailAlokasiCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory, ILogger<SaveDetailAlokasiCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveDetailAlokasiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var entity = await dbContext.DetailAlokasi.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_pol, request.no_updt, 
                    request.no_rsk, request.kd_endt, request.no_updt_reas, request.kd_jns_sor, 
                    request.kd_grp_sor, request.kd_rk_sor, request.kd_grp_sb_bis);
            
                if (entity == null)
                {
                    var detailAlokasi = _mapper.Map<DetailAlokasi>(request);
                    dbContext.DetailAlokasi.Add(detailAlokasi);

                    await dbContext.SaveChangesAsync(cancellationToken);

                }
                else
                {
                    entity.nilai_ttl_ptg_reas = request.nilai_ttl_ptg_reas;
                    entity.pst_share = request.pst_share;
                    entity.nilai_prm_reas = request.nilai_prm_reas;
                    entity.pst_kms_reas = request.pst_kms_reas;
                    entity.nilai_kms_reas = request.nilai_kms_reas;
                    entity.pst_adj_reas = request.pst_adj_reas;
                    entity.stn_adj_reas = request.stn_adj_reas;
                    entity.nilai_adj_reas = request.nilai_adj_reas;

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