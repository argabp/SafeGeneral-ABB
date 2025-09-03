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

        public int no_updt { get; set; }

        public int no_rsk { get; set; }

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
                    entity.kd_jns_kr = request.kd_jns_kr;
                    entity.st_pbyr = request.st_pbyr;
                    entity.kd_grp_kr = request.kd_grp_kr;
                    entity.kd_rk_asj = request.kd_rk_asj;
                    entity.no_deb = request.no_deb;
                    entity.no_sppa = request.no_sppa;
                    entity.nm_deb = request.nm_deb;
                    entity.kd_usr = request.kd_usr;
                    entity.kd_updt = request.kd_updt;
                    entity.alm_lok_ptg = request.alm_lok_ptg;
                    entity.tmp_lahir = request.tmp_lahir;
                    entity.tgl_lahir = request.tgl_lahir;
                    entity.usia_deb = request.usia_deb;
                    entity.tgl_real = request.tgl_real;
                    entity.tgl_mul_ptg = request.tgl_mul_ptg;
                    entity.tgl_akh_ptg = request.tgl_akh_ptg;
                    entity.jk_wkt = request.jk_wkt;
                    entity.thn_akh = request.thn_akh;
                    entity.tgl_input = request.tgl_input;
                    entity.waris01 = request.waris01;
                    entity.hub01 = request.hub01;
                    entity.waris02 = request.waris02;
                    entity.hub02 = request.hub02;
                    entity.flag_std = request.flag_std;
                    entity.nilai_harga_ptg = request.nilai_harga_ptg;
                    entity.pst_rate_std = request.pst_rate_std;
                    entity.stn_rate_std = request.stn_rate_std;
                    entity.nilai_prm_std = request.nilai_prm_std;
                    entity.nilai_prm_btn = request.nilai_prm_btn;
                    entity.flag_bjr = request.flag_bjr;
                    entity.nilai_ptg_bjr = request.nilai_ptg_bjr;
                    entity.pst_rate_bjr = request.pst_rate_bjr;
                    entity.stn_rate_bjr = request.stn_rate_bjr;
                    entity.nilai_prm_bjr = request.nilai_prm_bjr;
                    entity.nilai_bia_adm = request.nilai_bia_adm;
                    entity.flag_gb = request.flag_gb;
                    entity.nilai_ptg_gb = request.nilai_ptg_gb;
                    entity.pst_rate_gb = request.pst_rate_gb;
                    entity.stn_rate_gb = request.stn_rate_gb;
                    entity.nilai_prm_gb = request.nilai_prm_gb;
                    entity.flag_tl = request.flag_tl;
                    entity.nilai_ptg_tl = request.nilai_ptg_tl;
                    entity.pst_rate_tl = request.pst_rate_tl;
                    entity.stn_rate_tl = request.stn_rate_tl;
                    entity.nilai_prm_tl = request.nilai_prm_tl;
                    entity.stn_rate_phk = request.stn_rate_phk;
                    entity.nilai_ptg_phk = request.nilai_ptg_phk;
                    entity.pst_rate_phk = request.pst_rate_phk;
                    entity.stn_rate_phk = request.stn_rate_phk;
                    entity.nilai_prm_phk = request.nilai_prm_phk;
                    entity.no_endt = request.no_endt;
                    
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