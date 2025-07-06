using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Commands
{
    public class SaveAkseptasiCommand : IRequest<Akseptasi>, IMapFrom<Akseptasi>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public byte no_renew { get; set; }

        public decimal thn_uw { get; set; }

        public string no_endt { get; set; }

        public string? kd_updt { get; set; }

        public string? no_reg { get; set; }

        public string? no_pol_lama { get; set; }

        public string? kd_grp_ttg { get; set; }

        public string? kd_rk_ttg { get; set; }

        public string nm_ttg { get; set; }

        public string? almt_ttg { get; set; }

        public string? kt_ttg { get; set; }

        public string? nm_qq { get; set; }

        public string? kd_grp_brk { get; set; }

        public string? kd_rk_brk { get; set; }

        public string st_pas { get; set; }

        public string? kd_grp_pas { get; set; }

        public string? kd_rk_pas { get; set; }

        public string? kd_grp_bank { get; set; }

        public string? kd_rk_bank { get; set; }

        public string kd_grp_sb_bis { get; set; }

        public string kd_rk_sb_bis { get; set; }
        
        public DateTime tgl_mul_ptg { get; set; }

        public DateTime tgl_akh_ptg { get; set; }

        public decimal pst_share_bgu { get; set; }

        public Int16 jk_wkt_ptg { get; set; }

        public decimal faktor_prd { get; set; }

        public string? no_pol_pas { get; set; }

        public string? no_pol_induk { get; set; }

        public string? ctt_pol { get; set; }

        public string? lamp_pol { get; set; }

        public string? ket_endt { get; set; }

        public string? desk_deduct { get; set; }

        public string? ket_klausula { get; set; }

        public string? no_brdr { get; set; }

        public DateTime? tgl_brdr { get; set; }

        public string flag_konv { get; set; }

        public string? flag_reas { get; set; }

        public DateTime? tgl_ttd { get; set; }

        public DateTime tgl_input { get; set; }

        public string kd_usr_input { get; set; }

        public DateTime? tgl_updt { get; set; }

        public string? kd_usr_updt { get; set; }

        public DateTime? tgl_closing { get; set; }

        public string? kd_usr_closing { get; set; }

        public string? kd_grp_mkt { get; set; }

        public string? kd_rk_mkt { get; set; }

        public string? no_pol_ttg { get; set; }

        public string? kd_konfirm { get; set; }

        public string? no_survey { get; set; }

        public string? link_file { get; set; }

        public string? st_cover { get; set; }

        public DateTime? tgl_maintenance { get; set; }

        public byte? wpc { get; set; }

        public string? flag_dis_fleet { get; set; }

        public string? ket_no_reg { get; set; }

        public string? no_reff { get; set; }

        public string? st_aks { get; set; }

        public IFormFile file { get; set; }

        public string? nomor_pengajuan { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveAkseptasiCommand, Akseptasi>();
        }
    }

    public class SaveAkseptasiCommandHandler : IRequestHandler<SaveAkseptasiCommand, Akseptasi>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IProfilePictureHelper _profilePictureHelper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SaveAkseptasiCommandHandler> _logger;

        public SaveAkseptasiCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory, IProfilePictureHelper profilePictureHelper,
            IConfiguration configuration, ILogger<SaveAkseptasiCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _profilePictureHelper = profilePictureHelper;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Akseptasi> Handle(SaveAkseptasiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var entity = await dbContext.Akseptasi.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt);
            
                if (entity == null)
                {
                    request.kd_thn = DateTime.Now.ToString("yy");
                
                    _connectionFactory.CreateDbConnection(request.DatabaseName);
                    var no_aks = (await _connectionFactory.QueryProc<string>("spe_uw02e_01", new
                        {
                            request.kd_cb, request.kd_cob,
                            request.kd_scob, request.kd_thn
                        }))
                        .ToList();

                    var akseptasi = _mapper.Map<Akseptasi>(request);
                
                    akseptasi.no_aks = no_aks[0].Split(",")[1];
                    akseptasi.no_updt = 0;
                    akseptasi.no_endt = "0";
                
                    var path = _configuration.GetSection("Akseptasi").Value.TrimEnd('/');
                    path = Path.Combine(path, request.kd_cb + request.kd_cob + request.kd_scob + 
                                              request.kd_thn + akseptasi.no_aks + akseptasi.no_updt);
                
                    akseptasi.link_file = path;

                    await _connectionFactory.QueryProc<string>("spe_uw02e_25_01", new
                    {
                        request.wpc, nopol_induk = request.no_pol_induk
                    });
                
                    dbContext.Akseptasi.Add(akseptasi);

                    await dbContext.SaveChangesAsync(cancellationToken);
                
                    await _profilePictureHelper.UploadToFolder(request.file, path);

                    return akseptasi;
                }
                else
                {
                    _connectionFactory.CreateDbConnection(request.DatabaseName);
                    await _connectionFactory.QueryProc<string>("spe_uw02e_25_01", new
                    {
                        request.wpc, nopol_induk = request.no_pol_induk
                    });

                    entity.no_endt = request.no_endt;
                    entity.link_file = request.link_file;
                    entity.nomor_pengajuan = request.nomor_pengajuan;
                    entity.no_pol_induk = request.no_pol_induk;
                    entity.no_renew = request.no_renew;
                    entity.kd_updt = request.kd_updt;
                    entity.thn_uw = request.thn_uw;
                    entity.no_pol_lama = request.no_pol_lama;
                    entity.st_aks = request.st_aks;
                    entity.kd_grp_sb_bis = request.kd_grp_sb_bis;
                    entity.kd_rk_sb_bis = request.kd_rk_sb_bis;
                    entity.kd_grp_ttg = request.kd_grp_ttg;
                    entity.kd_rk_ttg = request.kd_rk_ttg;
                    entity.nm_ttg = request.nm_ttg;
                    entity.almt_ttg = request.almt_ttg;
                    entity.kt_ttg = request.kt_ttg;
                    entity.nm_qq = request.nm_qq;
                    entity.tgl_mul_ptg = request.tgl_mul_ptg;
                    entity.tgl_akh_ptg = request.tgl_akh_ptg;
                    entity.pst_share_bgu = request.pst_share_bgu;
                    entity.jk_wkt_ptg = request.jk_wkt_ptg;
                    entity.faktor_prd = request.faktor_prd;
                    entity.st_pas = request.st_pas;
                    entity.st_cover = request.st_cover;
                    entity.kd_grp_pas = request.kd_grp_pas;
                    entity.kd_rk_pas = request.kd_rk_pas;
                    entity.kd_grp_brk = request.kd_grp_brk;
                    entity.kd_rk_brk = request.kd_rk_brk;
                    entity.kd_grp_bank = request.kd_grp_bank;
                    entity.kd_rk_bank = request.kd_rk_bank;
                    entity.kd_grp_mkt = request.kd_grp_mkt;
                    entity.kd_rk_mkt = request.kd_rk_mkt;
                    entity.no_pol_pas = request.no_pol_pas;
                    entity.ctt_pol = request.ctt_pol;
                    entity.lamp_pol = request.lamp_pol;
                    entity.ket_klausula = request.ket_klausula;
                    entity.flag_konv = request.flag_konv;
                    entity.no_survey = request.no_survey;
                    entity.tgl_ttd = request.tgl_ttd;
                    entity.wpc = request.wpc;
                    entity.tgl_input = request.tgl_input;
                    entity.tgl_updt = request.tgl_updt;
                    entity.tgl_closing = request.tgl_closing;
                    entity.kd_usr_input = request.kd_usr_input;
                    entity.kd_usr_updt = request.kd_usr_updt;
                    entity.kd_usr_closing = request.kd_usr_closing;
                    
                    await dbContext.SaveChangesAsync(cancellationToken);

                    return entity;
                }
                
                
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException == null ? e.Message : e.InnerException.Message);
                throw e.InnerException ?? e;
            }
        }
    }
}