using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ABB.Application.RegisterKlaims.Commands
{
    public class SaveRegisterKlaimCommand : IRequest<RegisterKlaim>, IMapFrom<RegisterKlaim>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public string flag_pol_lama { get; set; }

        public string flag_tty_msk { get; set; }

        public string? no_pol_lama { get; set; }

        public string? kd_thn_pol { get; set; }

        public string? no_pol { get; set; }

        public Int16? no_updt { get; set; }

        public Int16? no_rsk { get; set; }

        public string? kd_jns_sor { get; set; }

        public string? kd_tty_msk { get; set; }

        public string? no_lks_lama { get; set; }

        public string flag_settled { get; set; }

        public string? st_jns_peny { get; set; }

        public string? ket_oby { get; set; }

        public DateTime tgl_lapor { get; set; }

        public DateTime tgl_kej { get; set; }

        public string? tempat_kej { get; set; }

        public string? sebab_kerugian { get; set; }

        public string? kond_ptg { get; set; }

        public string? kd_sebab { get; set; }

        public string? sifat_kerugian { get; set; }

        public DateTime tgl_lns_prm { get; set; }

        public string? no_bukti_lns { get; set; }

        public DateTime tgl_reg { get; set; }

        public string? ket_kl { get; set; }

        public DateTime tgl_input { get; set; }

        public string kd_usr_input { get; set; }

        public DateTime? tgl_updt { get; set; }

        public string? kd_usr_updt { get; set; }

        public string? pelapor { get; set; }

        public string? st_pelapor { get; set; }

        public string? no_tlp_pelapor { get; set; }

        public string? penerima { get; set; }

        public string st_reg { get; set; }

        public string? kd_wilayah { get; set; }

        public string? kd_grp_bkl { get; set; }

        public string? kd_rk_bkl { get; set; }

        public string? kd_pas { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveRegisterKlaimCommand, RegisterKlaim>();
        }
    }

    public class SaveRegisterKlaimCommandHandler : IRequestHandler<SaveRegisterKlaimCommand, RegisterKlaim>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IProfilePictureHelper _profilePictureHelper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SaveRegisterKlaimCommandHandler> _logger;
        private readonly ICurrentUserService _currentUserService;

        public SaveRegisterKlaimCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory, IProfilePictureHelper profilePictureHelper,
            IConfiguration configuration, ILogger<SaveRegisterKlaimCommandHandler> logger,
            ICurrentUserService currentUserService)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _profilePictureHelper = profilePictureHelper;
            _configuration = configuration;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<RegisterKlaim> Handle(SaveRegisterKlaimCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);

                var result = (await _connectionFactory.QueryProc<string>("spe_cl02e_14", new
                {
                    request.flag_pol_lama, request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn_pol,
                    request.no_pol, request.no_updt, request.no_pol_lama, request.no_kl, request.kd_thn, kd_jns_sor = request.kd_jns_sor ?? string.Empty,
                    request.flag_tty_msk, request.tgl_kej, request.kd_usr_input, kd_pass = request.kd_pas ?? string.Empty, 
                    request.st_jns_peny, request.sifat_kerugian, request.no_rsk, request.kd_wilayah
                })).FirstOrDefault();
                
                if (!string.IsNullOrWhiteSpace(result))
                    throw new Exception(result);
                
                var entity = await dbContext.RegisterKlaim.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_kl);
            
                if (entity == null)
                {
                    _connectionFactory.CreateDbConnection(request.DatabaseName);
                    var no_kl = (await _connectionFactory.QueryProc<string>("spe_cl01e_01", new
                        {
                            request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn
                        }))
                        .ToList();
                    
                    var registerKlaim = _mapper.Map<RegisterKlaim>(request);
                
                    registerKlaim.no_kl = no_kl[0].Split(",")[1];
                    registerKlaim.tgl_input = DateTime.Now;
                    registerKlaim.tgl_updt = DateTime.Now;
                    registerKlaim.kd_usr_input = _currentUserService.UserId;
                    registerKlaim.kd_usr_updt = _currentUserService.UserId;

                    dbContext.RegisterKlaim.Add(registerKlaim);

                    await dbContext.SaveChangesAsync(cancellationToken);

                    await _connectionFactory.QueryProc<string>("sp_InsertDokumenKlaim",
                        new { request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn, request.no_kl });

                    return registerKlaim;
                }
                else
                {
                    _connectionFactory.CreateDbConnection(request.DatabaseName);

                    entity.flag_settled = request.flag_settled;
                    entity.pelapor = request.pelapor;
                    entity.no_tlp_pelapor = request.no_tlp_pelapor;
                    entity.st_pelapor = request.st_pelapor;
                    entity.tgl_lapor = request.tgl_lapor;
                    entity.penerima = request.penerima;
                    entity.tgl_kej = request.tgl_kej;
                    entity.tempat_kej = request.tempat_kej;
                    entity.kd_wilayah = request.kd_wilayah;
                    entity.flag_tty_msk = request.flag_tty_msk;
                    entity.flag_pol_lama = request.flag_pol_lama;
                    entity.st_reg = request.st_reg;
                    entity.no_pol_lama = request.no_pol_lama;
                    entity.no_updt = request.no_updt;
                    entity.no_rsk = request.no_rsk;
                    entity.no_lks_lama = request.no_lks_lama;
                    entity.no_pol = request.no_pol;
                    entity.st_jns_peny = request.st_jns_peny;
                    entity.sifat_kerugian = request.sifat_kerugian;
                    entity.kd_sebab = request.kd_sebab;
                    entity.sebab_kerugian = request.sebab_kerugian;
                    entity.kd_grp_bkl = request.kd_grp_bkl;
                    entity.kond_ptg = request.kond_ptg;
                    entity.ket_kl = request.ket_kl;
                    entity.ket_oby = request.ket_oby;
                    entity.tgl_lns_prm = request.tgl_lns_prm;
                    entity.no_bukti_lns = request.no_bukti_lns;
                    entity.tgl_input = request.tgl_input;
                    entity.tgl_updt = request.tgl_updt;
                    entity.kd_usr_input = request.kd_usr_input;
                    entity.kd_usr_updt = request.kd_usr_updt;
                    entity.kd_thn_pol = request.kd_thn_pol;
                    
                    await dbContext.SaveChangesAsync(cancellationToken);

                    await _connectionFactory.QueryProc<string>("sp_InsertDokumenKlaim",
                        new { request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn, request.no_kl });

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