using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Commands
{
    public class SaveAkseptasiResikoCommand : IRequest, IMapFrom<AkseptasiResiko>
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

        public string? ket_rsk { get; set; }

        public string kd_mtu_prm { get; set; }

        public decimal pst_rate_prm { get; set; }

        public byte stn_rate_prm { get; set; }

        public decimal nilai_prm { get; set; }

        public decimal? pst_dis { get; set; }

        public decimal nilai_dis { get; set; }

        public decimal? pst_kms { get; set; }

        public decimal nilai_kms { get; set; }

        public decimal? nilai_insentif { get; set; }

        public decimal? nilai_kl { get; set; }

        public decimal nilai_ttl_ptg { get; set; }

        public Int16? jk_wkt_ptg { get; set; }

        public decimal? faktor_prd { get; set; }

        public decimal? pst_share_bgu { get; set; }

        public string? no_pol_ttg { get; set; }

        public string? kd_tol { get; set; }

        public string? kode { get; set; }

        public DateTime? tgl_mul_ptg { get; set; }

        public DateTime? tgl_akh_ptg { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveAkseptasiResikoCommand, AkseptasiResiko>();
        }
    }

    public class SaveAkseptasiResikoCommandHandler : IRequestHandler<SaveAkseptasiResikoCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SaveAkseptasiResikoCommandHandler> _logger;

        public SaveAkseptasiResikoCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory,
            IConfiguration configuration, ILogger<SaveAkseptasiResikoCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveAkseptasiResikoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);

                if (request.kd_cb.Length >= 4 && request.kd_cb.Substring(3, 1) != "0")
                {
                    var result = (await _connectionFactory.QueryProc<string>("spe_uw02e_25_02", new
                    {
                        request.kd_tol, request.kode, request.kd_cob
                    })).First();

                    if (!string.IsNullOrWhiteSpace(result))
                        throw new Exception(result);
                }
                else
                {
                    var result = (await _connectionFactory.QueryProc<string>("spe_uw02e_25_02", new
                    {
                        request.kd_tol, request.kode
                    })).First();

                    if (!string.IsNullOrWhiteSpace(result))
                        throw new Exception(result);
                }
                
                var entity = await dbContext.AkseptasiResiko.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                    request.no_rsk, request.kd_endt);
            
                if (entity == null)
                {
                    var akseptasiResiko = _mapper.Map<AkseptasiResiko>(request);

                    var no_rsk = (await _connectionFactory.QueryProc<string>("spe_uw02e_02", new
                        {
                            request.kd_cb, request.kd_cob, request.kd_scob, 
                            request.kd_thn, request.no_aks, request.no_updt
                        }))
                        .First();

                    akseptasiResiko.no_rsk = Convert.ToInt16(no_rsk.Split(",")[1]);
                    
                    dbContext.AkseptasiResiko.Add(akseptasiResiko);

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    entity.nilai_kl = request.nilai_kl;
                    entity.nilai_insentif = request.nilai_insentif;
                    entity.ket_rsk = request.ket_rsk;
                    entity.kd_mtu_prm = request.kd_mtu_prm;
                    entity.pst_rate_prm = request.pst_rate_prm;
                    entity.stn_rate_prm = request.stn_rate_prm;
                    entity.nilai_prm = request.nilai_prm;
                    entity.nilai_ttl_ptg = request.nilai_ttl_ptg;
                    entity.pst_dis = request.pst_dis;
                    entity.nilai_dis = request.nilai_dis;
                    entity.jk_wkt_ptg = request.jk_wkt_ptg;
                    entity.pst_kms = request.pst_kms;
                    entity.nilai_kms = request.nilai_kms;
                    entity.pst_share_bgu = request.pst_share_bgu;
                    entity.kd_tol = request.kd_tol;
                    entity.faktor_prd = request.faktor_prd;
                    entity.kode = request.kode;
                    entity.tgl_mul_ptg = request.tgl_mul_ptg;
                    entity.tgl_akh_ptg = request.tgl_akh_ptg;
            
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