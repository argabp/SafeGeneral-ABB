using System;
using System.IO;
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
    public class SaveAkseptasiOtherCargoCommand : IRequest, IMapFrom<AkseptasiOtherCargo>
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

        public string nm_kapal { get; set; }

        public string? grp_kond { get; set; }

        public string? kd_kond { get; set; }

        public decimal? pst_deduct { get; set; }

        public DateTime? tgl_brkt { get; set; }

        public string? no_bl { get; set; }

        public string? no_lc { get; set; }

        public string? no_inv { get; set; }

        public string tempat_brkt { get; set; }

        public string? tempat_tiba { get; set; }

        public string? tempat_transit { get; set; }

        public string? consignee { get; set; }

        public string? kond_sps { get; set; }

        public string? survey { get; set; }

        public string? no_po { get; set; }

        public string? no_pol_ttg { get; set; }

        public string kd_kapal { get; set; }

        public string? link_file { get; set; }

        public string? st_transit { get; set; }

        public IFormFile file { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveAkseptasiOtherCargoCommand, AkseptasiOtherCargo>();
        }
    }

    public class SaveAkseptasiOtherCargoCommandHandler : IRequestHandler<SaveAkseptasiOtherCargoCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IConfiguration _configuration;
        private readonly IProfilePictureHelper _profilePictureHelper;
        private readonly ILogger<SaveAkseptasiOtherCargoCommandHandler> _logger;

        public SaveAkseptasiOtherCargoCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            IDbConnectionFactory connectionFactory, IConfiguration configuration, IProfilePictureHelper profilePictureHelper,
            ILogger<SaveAkseptasiOtherCargoCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _connectionFactory = connectionFactory;
            _configuration = configuration;
            _profilePictureHelper = profilePictureHelper;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveAkseptasiOtherCargoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                
                var entity = await dbContext.AkseptasiOtherCargo.FindAsync(request.kd_cb, 
                    request.kd_cob, request.kd_scob, request.kd_thn, request.no_aks, request.no_updt, 
                    request.no_rsk, request.kd_endt);
            
                if (entity == null)
                {
                    var akseptasiOther = _mapper.Map<AkseptasiOtherCargo>(request);

                    var pathAkseptasi = _configuration.GetSection("Akseptasi").Value.TrimEnd('/');
                    var path = _configuration.GetSection("AkseptasiResikoOtherCargo").Value.TrimEnd('/');
                    path = Path.Combine(pathAkseptasi.Replace("/", ""), request.kd_cb + request.kd_cob + request.kd_scob + 
                                                       request.kd_thn + request.no_aks + request.no_updt, path.Replace("/", ""));

                    request.link_file = path;
                    
                    dbContext.AkseptasiOtherCargo.Add(akseptasiOther);

                    await dbContext.SaveChangesAsync(cancellationToken);
                
                    await _profilePictureHelper.UploadToFolder(request.file, path);
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