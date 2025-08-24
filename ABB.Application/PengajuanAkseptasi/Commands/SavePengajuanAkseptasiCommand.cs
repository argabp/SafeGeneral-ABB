using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PengajuanAkseptasi.Commands
{
    public class SavePengajuanAkseptasiCommand : IRequest<TRAkseptasi>, IMapFrom<TRAkseptasi>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }
        
        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public string nomor_pengajuan { get; set; }

        public string kd_grp_mkt { get; set; }

        public string kd_rk_mkt { get; set; }

        public string kd_grp_sb_bis { get; set; }

        public string kd_rk_sb_bis { get; set; }

        public string kd_grp_ttg { get; set; }

        public string kd_rk_ttg { get; set; }

        public DateTime tgl_mul_ptg { get; set; }
        
        public DateTime tgl_akh_ptg { get; set; }

        public string kd_mtu { get; set; }

        public decimal nilai_ttl_ptg { get; set; }

        public string st_pas { get; set; }

        public decimal pst_share { get; set; }

        public string? no_pol_pas { get; set; }

        public string? kd_grp_pas1 { get; set; }

        public string? kd_rk_pas1 { get; set; }

        public decimal? pst_pas1 { get; set; }

        public string? kd_grp_pas2 { get; set; }

        public string? kd_rk_pas2 { get; set; }

        public decimal? pst_pas2 { get; set; }

        public string? kd_grp_pas3 { get; set; }

        public string? kd_rk_pas3 { get; set; }

        public decimal? pst_pas3 { get; set; }

        public string? kd_grp_pas4 { get; set; }

        public string? kd_rk_pas4 { get; set; }

        public decimal? pst_pas4 { get; set; }

        public string? kd_grp_pas5 { get; set; }

        public string? kd_rk_pas5 { get; set; }

        public decimal? pst_pas5 { get; set; }

        public string ket_rsk { get; set; }

        public DateTime tgl_pengajuan { get; set; }

        public decimal? pst_dis { get; set; }
        
        public decimal? pst_kms { get; set; }

        public bool? flag_approved { get; set; }

        public decimal? nilai_ttl_ptg_limit { get; set; }
        
        public string? kd_tol { get; set; }

        public decimal? pst_tol { get; set; }

        public decimal? pst_koas { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SavePengajuanAkseptasiCommand, TRAkseptasi>();
        }
    }

    public class SavePengajuanAkseptasiCommandHandler : IRequestHandler<SavePengajuanAkseptasiCommand, TRAkseptasi>
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<SavePengajuanAkseptasiCommandHandler> _logger;
        private readonly ICurrentUserService _user;
        private readonly IDbConnectionFactory _connectionFactory;

        public SavePengajuanAkseptasiCommandHandler(IDbContext context, IMapper mapper, 
            ILogger<SavePengajuanAkseptasiCommandHandler> logger, ICurrentUserService user, IDbConnectionFactory connectionFactory)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _user = user;
            _connectionFactory = connectionFactory;
        }

        public async Task<TRAkseptasi> Handle(SavePengajuanAkseptasiCommand request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.kd_thn))
            {
                request.kd_thn = request.tgl_pengajuan.ToString("yy");
            }

            var trAkseptasi = _mapper.Map<TRAkseptasi>(request);

            try
            {
                await _connectionFactory.QueryProc<string>("sp_ValidasiAkseptasi",
                    new { request.kd_cob, request.kd_scob, request.kd_thn, request.pst_dis, request.pst_kms });
                
                var entity = _context.TRAkseptasi.FirstOrDefault(w => w.kd_cb == request.kd_cb &&
                                                                      w.kd_cob == request.kd_cob
                                                                      && w.kd_scob == request.kd_scob &&
                                                                      w.kd_thn == request.kd_thn
                                                                      && w.no_aks == request.no_aks);


                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var no_aks = (await _connectionFactory.QueryProc<string>("sp_MaksNomorAks",
                    new { request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn })).FirstOrDefault();

                var nomor_pengajuan = (await _connectionFactory.QueryProc<string>("sp_GenerateNomorPengajuanAks",
                    new { request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn, no_aks })).FirstOrDefault();

                var dateNow = DateTime.Now;
                if (entity == null)
                {
                    
                    trAkseptasi.no_aks = no_aks;
                    trAkseptasi.nomor_pengajuan = nomor_pengajuan;
                    trAkseptasi.tgl_input = dateNow;
                    trAkseptasi.kd_status = 1;
                    trAkseptasi.kd_user_status = _user.UserId;
                    trAkseptasi.kd_user_input = _user.UserId;
                    trAkseptasi.tgl_status = dateNow;
                    trAkseptasi.kd_user_update = string.Empty;
                    trAkseptasi.tgl_update = dateNow;
                    trAkseptasi.flag_approved = false;
                    trAkseptasi.flag_closing = "N";

                    _context.TRAkseptasi.Add(trAkseptasi);
                }
                else
                {
                    entity.kd_grp_mkt = request.kd_grp_mkt;
                    entity.kd_rk_mkt = request.kd_rk_mkt;
                    entity.kd_grp_sb_bis = request.kd_grp_sb_bis;
                    entity.kd_rk_sb_bis = request.kd_rk_sb_bis;
                    entity.kd_grp_ttg = request.kd_grp_ttg;
                    entity.kd_rk_ttg = request.kd_rk_ttg;
                    entity.ket_rsk = request.ket_rsk;
                    entity.tgl_mul_ptg = request.tgl_mul_ptg;
                    entity.tgl_akh_ptg = request.tgl_akh_ptg;
                    entity.kd_mtu = request.kd_mtu;
                    entity.nilai_ttl_ptg = request.nilai_ttl_ptg;
                    entity.st_pas = request.st_pas;
                    entity.pst_share = request.pst_share;
                    entity.no_pol_pas = request.no_pol_pas;
                    entity.kd_user_update = _user.UserId;
                    entity.tgl_update = DateTime.Now;
                    entity.kd_grp_pas1 = request.kd_grp_pas1;
                    entity.kd_rk_pas1 = request.kd_rk_pas1;
                    entity.pst_pas1 = request.pst_pas1;
                    entity.kd_grp_pas2 = request.kd_grp_pas2;
                    entity.kd_rk_pas2 = request.kd_rk_pas2;
                    entity.pst_pas2 = request.pst_pas2;
                    entity.kd_grp_pas3 = request.kd_grp_pas3;
                    entity.kd_rk_pas3 = request.kd_rk_pas3;
                    entity.pst_pas3 = request.pst_pas3;
                    entity.kd_grp_pas4 = request.kd_grp_pas4;
                    entity.kd_rk_pas4 = request.kd_rk_pas4;
                    entity.pst_pas4 = request.pst_pas4;
                    entity.kd_grp_pas5 = request.kd_grp_pas5;
                    entity.kd_rk_pas5 = request.kd_rk_pas5;
                    entity.pst_pas5 = request.pst_pas5;
                    entity.pst_dis = request.pst_dis;
                    entity.pst_kms = request.pst_kms;
                    entity.tgl_pengajuan = request.tgl_pengajuan;
                    entity.nilai_ttl_ptg_limit = request.nilai_ttl_ptg_limit;
                    entity.kd_tol = request.kd_tol;
                    entity.pst_tol = request.pst_tol;
                    entity.pst_koas = request.pst_koas;
                }

                await _context.SaveChangesAsync(cancellationToken);

                await _connectionFactory.QueryProc<string>("sp_InsertDokumenPengajuanAks",
                    new { request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn, no_aks });
                
                await _connectionFactory.QueryProc<string>("sp_InsertLimitPengajuanAks",
                    new { request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn, no_aks });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                throw ex;
            }

            return trAkseptasi;
        }
    }
}