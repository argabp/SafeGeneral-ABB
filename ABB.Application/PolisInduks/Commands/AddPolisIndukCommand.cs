using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PolisInduks.Commands
{
    public class AddPolisIndukCommand : IRequest, IMapFrom<PolisInduk>
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
            profile.CreateMap<AddPolisIndukCommand, PolisInduk>();
        }
    }

    public class AddPolisIndukCommandHandler : IRequestHandler<AddPolisIndukCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<AddPolisIndukCommandHandler> _logger;

        public AddPolisIndukCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory, ILogger<AddPolisIndukCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddPolisIndukCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                request.kd_thn = DateTime.Now.ToString("yy");
                
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var no_pol = (await _connectionFactory.QueryProc<string>("spe_uw02e_27", new
                    {
                        request.kd_cb, request.kd_cob,
                        request.kd_scob, request.kd_thn,
                        no_pol = string.Empty, request.flag_konv
                    }))
                    .ToList();

                var polisInduk = _mapper.Map<PolisInduk>(request);

                polisInduk.no_pol = no_pol.Count > 0 ? no_pol[0].Split(",")[1] : string.Empty;
                polisInduk.kd_pkk_sb_bis = "0";
                polisInduk.st_pol = "0";

                await _connectionFactory.QueryProc<string>("spe_uw02e_96", new
                {
                    request.kd_scob, polisInduk.no_pol,
                    request.nilai_tsi, request.pst_kms,
                    request.pst_dis, kd_konfirm_induk = request.kd_konfirm,
                    request.kd_mtu, request.kd_cob,
                    request.kd_thn
                });
                
                dbContext.PolisInduk.Add(polisInduk);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException == null ? e.Message : e.InnerException.Message);
                throw e.InnerException ?? e;
            }
            
            return Unit.Value;
        }
    }
}