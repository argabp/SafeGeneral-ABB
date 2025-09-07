using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PolisInduks.Commands
{
    public class EditPolisIndukCommand : IRequest, IMapFrom<PolisInduk>
    {
        public string DatabaseName { get; set; }
        public string no_pol_induk { get; set; }

        public string st_pol { get; set; }

        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public decimal thn_uw { get; set; }

        public string kd_grp_ttg { get; set; }
        
        public string kd_rk_ttg { get; set; }

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

        public string kd_pkk_sb_bis { get; set; }

        public string kd_grp_sb_bis { get; set; }

        public string kd_rk_sb_bis { get; set; }

        public DateTime tgl_mul_ptg { get; set; }
        
        public DateTime tgl_akh_ptg { get; set; }

        public decimal pst_share_bgu { get; set; }

        public Int16 jk_wkt_ptg { get; set; }

        public decimal faktor_prd { get; set; }

        public decimal pst_prm { get; set; }

        public byte stn_rate_prm { get; set; }

        public decimal pst_dis { get; set; }

        public decimal pst_kms { get; set; }

        public decimal pst_insentif { get; set; }

        public decimal nilai_min_prm { get; set; }

        public string? no_pol_pas { get; set; }

        public string? ctt_pol { get; set; }

        public string? lamp_pol { get; set; }

        public string? ket_endt { get; set; }

        public string? desk_deduct { get; set; }

        public string? ket_klausula { get; set; }

        public string flag_konv { get; set; }

        public DateTime? tgl_ttd { get; set; }

        public DateTime tgl_input { get; set; }

        public string kd_usr_input { get; set; }

        public DateTime? tgl_updt { get; set; }

        public string? kd_usr_updt { get; set; }

        public string? kd_grp_mkt { get; set; }

        public string? kd_rk_mkt { get; set; }

        public decimal? nilai_deposit { get; set; }

        public decimal? nilai_tsi { get; set; }

        public byte? wpc { get; set; }

        public string? kd_mtu { get; set; }

        public string? kd_konfirm { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<EditPolisIndukCommand, PolisInduk>();
        }
    }

    public class EditPolisIndukCommandHandler : IRequestHandler<EditPolisIndukCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<EditPolisIndukCommandHandler> _logger;

        public EditPolisIndukCommandHandler(IDbContextFactory contextFactory, IMapper mapper, 
            IDbConnectionFactory connectionFactory, ILogger<EditPolisIndukCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditPolisIndukCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var entity = await dbContext.PolisInduk.FindAsync(request.no_pol_induk);
            
                if (entity == null)
                    throw new NotFoundException();
            
                _connectionFactory.CreateDbConnection(request.DatabaseName);
            
                await _connectionFactory.QueryProc<string>("spe_uw02e_96", new
                {
                    request.kd_scob, request.no_pol,
                    request.nilai_tsi, request.pst_kms,
                    request.pst_dis, kd_konfirm_induk = request.kd_konfirm,
                    request.kd_mtu, request.kd_cob,
                    request.kd_thn
                });
            
                _mapper.Map(request, entity);

                await dbContext.SaveChangesAsync(cancellationToken);
            
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