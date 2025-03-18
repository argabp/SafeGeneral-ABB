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
    public class SaveAkseptasiOtherPACommand : IRequest, IMapFrom<AkseptasiOtherPA>
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

        public string? no_sppa { get; set; }

        public string? no_deb { get; set; }

        public string? nm_deb { get; set; }

        public string? alm_lok_ptg { get; set; }

        public string? waris01 { get; set; }

        public string? waris02 { get; set; }

        public string? hub01 { get; set; }

        public string? hub02 { get; set; }

        public string? kd_jns_kr { get; set; }

        public decimal? jk_wkt { get; set; }

        public DateTime? tgl_real { get; set; }

        public DateTime? tgl_mul_ptg { get; set; }

        public DateTime? tgl_akh_ptg { get; set; }

        public string? thn_akh { get; set; }

        public string? kd_grp_kr { get; set; }

        public decimal? nilai_harga_ptg { get; set; }

        public decimal? pst_rate_std { get; set; }

        public decimal? pst_rate_bjr { get; set; }

        public decimal? pst_rate_tl { get; set; }

        public decimal? pst_rate_gb { get; set; }

        public decimal? nilai_prm_std { get; set; }

        public decimal? nilai_prm_bjr { get; set; }

        public decimal? nilai_prm_tl { get; set; }

        public decimal? nilai_prm_gb { get; set; }

        public decimal? nilai_bia_adm { get; set; }

        public decimal? nilai_prm_btn { get; set; }

        public DateTime? tgl_input { get; set; }

        public string? st_pbyr { get; set; }

        public string? no_endt { get; set; }

        public string? kd_usr { get; set; }

        public string? kd_updt { get; set; }

        public string? flag_std { get; set; }
        
        public string? flag_bjr { get; set; }
        
        public string? flag_tl { get; set; }
        
        public string? flag_gb { get; set; }

        public DateTime? tgl_lahir { get; set; }

        public Int16? usia_deb { get; set; }

        public decimal? pst_rate_phk { get; set; }

        public decimal? nilai_prm_phk { get; set; }

        public decimal? nilai_bia_mat { get; set; }

        public decimal? nilai_ptg_std { get; set; }

        public decimal? nilai_ptg_bjr { get; set; }

        public decimal? nilai_ptg_tl { get; set; }

        public decimal? nilai_ptg_gb { get; set; }

        public decimal? nilai_ptg_hh { get; set; }

        public decimal? nilai_ptg_phk { get; set; }

        public string? no_pol_ttg { get; set; }

        public byte? stn_rate_std { get; set; }

        public byte? stn_rate_bjr { get; set; }

        public byte? stn_rate_gb { get; set; }

        public byte? stn_rate_tl { get; set; }

        public byte? stn_rate_phk { get; set; }

        public string? kd_grp_asj { get; set; }

        public string? kd_rk_asj { get; set; }

        public string? tmp_lahir { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveAkseptasiOtherPACommand, AkseptasiOtherPA>();
        }
    }

    public class SaveAkseptasiOtherPACommandHandler : IRequestHandler<SaveAkseptasiOtherPACommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveAkseptasiOtherPACommandHandler> _logger;

        public SaveAkseptasiOtherPACommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory, ILogger<SaveAkseptasiOtherPACommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveAkseptasiOtherPACommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var entity = await dbContext.AkseptasiOtherPA.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                    request.no_rsk, request.kd_endt);
            
                if (entity == null)
                {
                    var akseptasiOther = _mapper.Map<AkseptasiOtherPA>(request);

                    dbContext.AkseptasiOtherPA.Add(akseptasiOther);

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