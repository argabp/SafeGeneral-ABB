using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ImportAsumsi.Queries
{
    public class UploadAsumsiQuery : IRequest
    {
        public IFormFile File { get; set; }
    }
    
    public class UploadAsumsiQueryHandler : IRequestHandler<UploadAsumsiQuery>
    {
        private readonly IDbConnectionCSM _dbConnectionCsm;
        private readonly ILogger<UploadAsumsiQueryHandler> _logger;
        private readonly IProfilePictureHelper _pictureHelper;
        private readonly IConfiguration _configuration;

        public UploadAsumsiQueryHandler(IDbConnectionCSM dbConnectionCsm, ILogger<UploadAsumsiQueryHandler> logger,
            IProfilePictureHelper pictureHelper, IConfiguration configuration)
        {
            _dbConnectionCsm = dbConnectionCsm;
            _logger = logger;
            _pictureHelper = pictureHelper;
            _configuration = configuration;
        }
    
        public async Task<Unit> Handle(UploadAsumsiQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var asumsiPath = _configuration.GetSection("Asumsi").Value.TrimEnd('/');
                
                var path = await _pictureHelper.UploadToFolder(request.File, asumsiPath);
                
                await _dbConnectionCsm.QueryProc("sp_UploadAsumsi",
                    new
                    {
                        Path = path
                    });
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException == null ? e.Message : e.InnerException.Message);
                throw;
            }
            
            return Unit.Value;
        }
    }
}