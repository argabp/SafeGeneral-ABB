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
                    entity.kd_kapal = request.kd_kapal;
                    entity.nm_kapal = request.nm_kapal;
                    entity.no_bl = request.no_bl;
                    entity.no_po = request.no_po;
                    entity.no_inv = request.no_inv;
                    entity.no_lc = request.no_lc;
                    entity.tgl_brkt = request.tgl_brkt;
                    entity.tempat_brkt = request.tempat_brkt;
                    entity.tempat_tiba = request.tempat_tiba;
                    entity.tempat_transit = request.tempat_transit;
                    entity.consignee = request.consignee;
                    entity.kd_kond = request.kd_kond;
                    entity.kond_sps = request.kond_sps;
                    entity.survey = request.survey;
                    entity.st_transit = request.st_transit;
                    
                    var pathAkseptasi = _configuration.GetSection("Akseptasi").Value.TrimEnd('/');
                    var path = _configuration.GetSection("AkseptasiResikoOtherCargo").Value.TrimEnd('/');
                    path = Path.Combine(pathAkseptasi.Replace("/", ""), request.kd_cb + request.kd_cob + request.kd_scob + 
                                                                        request.kd_thn + request.no_aks + request.no_updt, path.Replace("/", ""));
                    
                    entity.link_file = path;
            
                    await dbContext.SaveChangesAsync(cancellationToken);

                    await _profilePictureHelper.UploadToFolder(request.file, path);
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