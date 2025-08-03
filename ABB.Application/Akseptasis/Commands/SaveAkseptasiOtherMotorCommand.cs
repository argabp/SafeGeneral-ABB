using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Commands
{
    public class SaveAkseptasiOtherMotorCommand : IRequest, IMapFrom<AkseptasiOtherMotor>
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

        public string? no_pls { get; set; }

        public string? grp_jns_kend { get; set; }

        public string kd_jns_kend { get; set; }

        public string? grp_merk_kend { get; set; }

        public string kd_merk_kend { get; set; }

        public string? tipe_kend { get; set; }

        public string? warna_kend { get; set; }

        public string no_rangka { get; set; }

        public string no_msn { get; set; }

        public decimal? thn_buat { get; set; }

        public byte? jml_tempat_ddk { get; set; }

        public string kd_jns_ptg { get; set; }

        public string kd_guna { get; set; }

        public string kd_utk { get; set; }

        public decimal nilai_casco { get; set; }

        public decimal nilai_tjh { get; set; }
        
        public decimal nilai_tjp { get; set; }

        public decimal nilai_pap { get; set; }
        
        public decimal nilai_pad { get; set; }

        public decimal pst_rate_prm { get; set; }

        public byte stn_rate_prm { get; set; }

        public decimal pst_rate_hh { get; set; }

        public byte stn_rate_hh { get; set; }

        public decimal nilai_rsk_sendiri { get; set; }

        public decimal nilai_prm_casco { get; set; }

        public decimal nilai_prm_tjh { get; set; }

        public decimal nilai_prm_tjp { get; set; }

        public decimal nilai_prm_pap { get; set; }

        public decimal nilai_prm_pad { get; set; }

        public decimal nilai_prm_hh { get; set; }

        public DateTime? tgl_mul_ptg { get; set; }

        public DateTime? tgl_akh_ptg { get; set; }

        public string? no_pinj { get; set; }

        public decimal? nilai_bia_pol { get; set; }

        public decimal? nilai_bia_mat { get; set; }

        public string? nm_qq { get; set; }

        public string? almt_qq { get; set; }

        public string? kt_qq { get; set; }

        public decimal? nilai_pap_med { get; set; }
        
        public decimal? nilai_pad_med { get; set; }

        public decimal? nilai_prm_pap_med { get; set; }

        public decimal? nilai_prm_pad_med { get; set; }

        public decimal nilai_prm_aog { get; set; }

        public decimal pst_rate_aog { get; set; }

        public byte stn_rate_aog { get; set; }

        public string? no_pol_ttg { get; set; }

        public decimal? pst_rate_banjir { get; set; }

        public byte? stn_rate_banjir { get; set; }

        public decimal? nilai_prm_banjir { get; set; }

        public string? kd_wilayah { get; set; }

        public string validitas { get; set; }

        public byte? stn_rate_trs { get; set; }

        public decimal? pst_rate_trs { get; set; }

        public decimal? nilai_prm_trs { get; set; }

        public string? flag_hh { get; set; }
        
        public string? flag_aog { get; set; }
        
        public string? flag_banjir { get; set; }
        
        public string? flag_trs { get; set; }

        public byte? stn_rate_tjh { get; set; }

        public decimal? pst_rate_tjh { get; set; }

        public byte? stn_rate_tjp { get; set; }

        public decimal? pst_rate_tjp { get; set; }

        public byte? stn_rate_pap { get; set; }

        public decimal? pst_rate_pap { get; set; }

        public byte? stn_rate_pad { get; set; }

        public decimal? pst_rate_pad { get; set; }

        public byte? jml_pap { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveAkseptasiOtherMotorCommand, AkseptasiOtherMotor>();
        }
    }

    public class SaveAkseptasiOtherMotorCommandHandler : IRequestHandler<SaveAkseptasiOtherMotorCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SaveAkseptasiOtherMotorCommandHandler> _logger;

        public SaveAkseptasiOtherMotorCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory, ILogger<SaveAkseptasiOtherMotorCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveAkseptasiOtherMotorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var entity = await dbContext.AkseptasiOtherMotor.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                    request.no_rsk, request.kd_endt);
                
                var result = (await _connectionFactory.QueryProc<string>("spe_uw02e_50", new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn,
                    request.no_aks, request.no_updt, no_polisi = request.no_pls, request.no_rangka,
                    request.no_msn, request.kd_jns_kend, request.kd_wilayah, request.kd_jns_ptg,
                    request.nilai_casco, request.pst_rate_prm, request.flag_banjir, request.flag_aog,
                    request.flag_hh, request.flag_trs, request.nilai_tjh, request.nilai_tjp,
                    request.nilai_pap, request.nilai_pad, request.pst_rate_banjir, request.pst_rate_aog,
                    request.pst_rate_hh, request.pst_rate_trs, request.pst_rate_tjh, request.pst_rate_tjp,
                    request.pst_rate_pap, request.pst_rate_pad
                })).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(result))
                    throw new Exception(result);
                
                if (entity == null)
                {
                    var akseptasiOther = _mapper.Map<AkseptasiOtherMotor>(request);

                    dbContext.AkseptasiOtherMotor.Add(akseptasiOther);

                    await dbContext.SaveChangesAsync(cancellationToken);

                }
                else
                {
                    entity.thn_buat = request.thn_buat;
                    entity.warna_kend = request.warna_kend;
                    entity.kd_jns_kend = request.kd_jns_kend;
                    entity.jml_tempat_ddk = request.jml_tempat_ddk;
                    entity.grp_merk_kend = request.grp_merk_kend;
                    entity.jml_pap = request.jml_pap;
                    entity.kd_merk_kend = request.kd_merk_kend;
                    entity.kd_jns_ptg = request.kd_jns_ptg;
                    entity.no_pls = request.no_pls;
                    entity.kd_utk = request.kd_utk;
                    entity.no_rangka = request.no_rangka;
                    entity.kd_guna = request.kd_guna;
                    entity.no_msn = request.no_msn;
                    entity.no_pinj = request.no_pinj;
                    entity.tgl_mul_ptg = request.tgl_mul_ptg;
                    entity.tgl_akh_ptg = request.tgl_akh_ptg;
                    entity.nm_qq = request.nm_qq;
                    entity.almt_qq = request.almt_qq;
                    entity.nilai_bia_pol = request.nilai_bia_pol;
                    entity.kt_qq = request.kt_qq;
                    entity.validitas = request.validitas;
                    entity.kd_wilayah = request.kd_wilayah;
                    entity.nilai_casco = request.nilai_casco;
                    entity.pst_rate_prm = request.pst_rate_prm;
                    entity.stn_rate_prm = request.stn_rate_prm;
                    entity.nilai_prm_casco = request.nilai_prm_casco;
                    entity.nilai_tjp = request.nilai_tjp;
                    entity.pst_rate_tjp = request.pst_rate_tjp;
                    entity.stn_rate_tjp = request.stn_rate_tjp;
                    entity.nilai_prm_tjp = request.nilai_prm_tjp;
                    entity.flag_hh = request.flag_hh;
                    entity.pst_rate_hh = request.pst_rate_hh;
                    entity.stn_rate_hh = request.stn_rate_hh;
                    entity.nilai_prm_hh = request.nilai_prm_hh;
                    entity.nilai_pap = request.nilai_pap;
                    entity.pst_rate_pap = request.pst_rate_pap;
                    entity.stn_rate_pap = request.stn_rate_pap;
                    entity.nilai_prm_pap = request.nilai_prm_pap;
                    entity.flag_aog = request.flag_aog;
                    entity.pst_rate_aog = request.pst_rate_aog;
                    entity.stn_rate_aog = request.stn_rate_aog;
                    entity.nilai_prm_aog = request.nilai_prm_aog;
                    entity.nilai_pad = request.nilai_pad;
                    entity.pst_rate_pad = request.pst_rate_pad;
                    entity.stn_rate_pad = request.stn_rate_pad;
                    entity.nilai_prm_pad = request.nilai_prm_pad;
                    entity.flag_banjir = request.flag_banjir;
                    entity.pst_rate_banjir = request.pst_rate_banjir;
                    entity.stn_rate_banjir = request.stn_rate_banjir;
                    entity.nilai_prm_banjir = request.nilai_prm_banjir;
                    entity.flag_trs = request.flag_trs;
                    entity.pst_rate_trs = request.pst_rate_trs;
                    entity.stn_rate_trs = request.stn_rate_trs;
                    entity.nilai_prm_trs = request.nilai_prm_trs;
                    entity.nilai_tjh = request.nilai_tjh;
                    entity.pst_rate_tjh = request.pst_rate_tjh;
                    entity.stn_rate_tjh = request.stn_rate_tjh;
                    entity.nilai_prm_tjh = request.nilai_prm_tjh;
                    entity.nilai_rsk_sendiri = request.nilai_rsk_sendiri;
            
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