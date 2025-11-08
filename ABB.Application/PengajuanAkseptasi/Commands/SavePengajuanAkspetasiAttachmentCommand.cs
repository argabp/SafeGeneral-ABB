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

namespace ABB.Application.PengajuanAkseptasi.Commands
{
    public class SavePengajuanAkspetasiAttachmentCommand : IRequest, IMapFrom<TRAkseptasiAttachment>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }
        
        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 kd_dokumen { get; set; }

        public string nm_dokumen { get; set; }

        public bool? flag_wajib { get; set; }
        
        public IFormFile File { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SavePengajuanAkspetasiAttachmentCommand, TRAkseptasiAttachment>();
        }
    }

    public class SavePengajuanAkspetasiAttachmentCommandHandler : IRequestHandler<SavePengajuanAkspetasiAttachmentCommand>
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<SavePengajuanAkspetasiAttachmentCommandHandler> _logger;
        private readonly ICurrentUserService _user;
        private readonly IProfilePictureHelper _pictureHelper;
        private readonly IConfiguration _config;

        public SavePengajuanAkspetasiAttachmentCommandHandler(IDbContext context, IMapper mapper, IDbConnection dbConnection,
                                        ILogger<SavePengajuanAkspetasiAttachmentCommandHandler> logger, ICurrentUserService user,
                                        IProfilePictureHelper pictureHelper, IConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _dbConnection = dbConnection;
            _logger = logger;
            _user = user;
            _pictureHelper = pictureHelper;
            _config = config;
        }

        public async Task<Unit> Handle(SavePengajuanAkspetasiAttachmentCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var entity = _context.TRAkseptasiAttachment.FirstOrDefault(w => w.kd_cb == request.kd_cb &&
                                                                          w.kd_cob == request.kd_cob
                                                                          && w.kd_scob == request.kd_scob &&
                                                                          w.kd_thn == request.kd_thn
                                                                          && w.no_aks == request.no_aks
                                                                          && w.kd_dokumen == request.kd_dokumen);
                
                var nomor_pengajuan = _context.TRAkseptasi.FirstOrDefault(w => w.kd_cb == request.kd_cb &&
                                                                   w.kd_cob == request.kd_cob
                                                                   && w.kd_scob == request.kd_scob &&
                                                                   w.kd_thn == request.kd_thn 
                                                                   && w.no_aks == request.no_aks)?.nomor_pengajuan;
                
                if (entity == null)
                {
                    var akseptasiAttachment = _mapper.Map<TRAkseptasiAttachment>(request);

                    akseptasiAttachment.nm_dokumen = request.File.FileName;
                    
                    _context.TRAkseptasiAttachment.Add(akseptasiAttachment);
                }
                else
                {
                    if (request.File != null)
                    {
                        entity.nm_dokumen = request.File.FileName;
                    }
                    entity.flag_wajib = request.flag_wajib;
                }

                await _context.SaveChangesAsync(cancellationToken);

                var path = _config.GetSection("PengajuanAkseptasiAttachment").Value.TrimEnd('/');
                path = Path.Combine(path, nomor_pengajuan.Replace("/", string.Empty));
                
                await _pictureHelper.UploadToFolder(request.File, path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Unit.Value;
        }
    }
}