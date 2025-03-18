using System;
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
                
                var entity = await dbContext.AkseptasiResiko.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                    request.no_rsk, request.kd_endt);
            
                if (entity == null)
                {
                    var akseptasiResiko = _mapper.Map<AkseptasiResiko>(request);

                    dbContext.AkseptasiResiko.Add(akseptasiResiko);

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