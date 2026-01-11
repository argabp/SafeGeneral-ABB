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
    public class SaveDokumenRegisterKlaimCommand : IRequest, IMapFrom<DokumenRegisterKlaim>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public string kd_dok { get; set; }

        public string? flag_dok { get; set; }

        public string? link_file { get; set; }

        public bool? flag_wajib { get; set; }
        
        public IFormFile File { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveDokumenRegisterKlaimCommand, DokumenRegisterKlaim>();
        }
    }

    public class SaveDokumenRegisterKlaimCommandHandler : IRequestHandler<SaveDokumenRegisterKlaimCommand>
    {
        private readonly IDbContextFactory _context;
        private readonly IMapper _mapper;
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<SaveDokumenRegisterKlaimCommandHandler> _logger;
        private readonly ICurrentUserService _user;
        private readonly IProfilePictureHelper _pictureHelper;
        private readonly IConfiguration _config;

        public SaveDokumenRegisterKlaimCommandHandler(IDbContextFactory context, IMapper mapper, IDbConnection dbConnection,
                                        ILogger<SaveDokumenRegisterKlaimCommandHandler> logger, ICurrentUserService user,
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

        public async Task<Unit> Handle(SaveDokumenRegisterKlaimCommand request, CancellationToken cancellationToken)
        {
            var dbContext = _context.CreateDbContext(request.DatabaseName);
            
            try
            {
                var entity = dbContext.DokumenRegisterKlaim.FirstOrDefault(w => w.kd_cb == request.kd_cb &&
                                                                          w.kd_cob == request.kd_cob
                                                                          && w.kd_scob == request.kd_scob &&
                                                                          w.kd_thn == request.kd_thn
                                                                          && w.no_kl == request.no_kl
                                                                          && w.kd_dok == request.kd_dok);
                
                if (entity == null)
                {
                    entity = _mapper.Map<DokumenRegisterKlaim>(request);

                    entity.link_file = request.File.FileName;
                    
                    dbContext.DokumenRegisterKlaim.Add(entity);
                }
                else
                {
                    if (request.File != null)
                    {
                        entity.link_file = request.File.FileName;
                    }
                    entity.flag_wajib = request.flag_wajib;
                }

                await dbContext.SaveChangesAsync(cancellationToken);

                var registerKlaimPath =
                    $"{entity.kd_cb.Trim()}{entity.kd_cob.Trim()}{entity.kd_scob.Trim()}{entity.kd_thn.Trim()}{entity.no_kl.Trim()}";
                var path = _config.GetSection("DokumenRegisterKlaim").Value.TrimEnd('/');
                path = Path.Combine(path, registerKlaimPath);
                
                await _pictureHelper.UploadToFolder(request.File, path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }

            return Unit.Value;
        }
    }
}