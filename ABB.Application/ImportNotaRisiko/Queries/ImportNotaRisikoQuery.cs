using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ABB.Application.ImportNotaRisiko.Queries
{
    public class ImportNotaRisikoQuery : IRequest
    {
        public IFormFile File { get; set; }
    }
    
    public class ImportNotaRisikoQueryHandler : IRequestHandler<ImportNotaRisikoQuery>
    {
        private readonly IDbConnectionCSM _dbConnectionCsm;
        private readonly ILogger<ImportNotaRisikoQueryHandler> _logger;
        private readonly IProfilePictureHelper _pictureHelper;
        private readonly IConfiguration _configuration;

        public ImportNotaRisikoQueryHandler(IDbConnectionCSM dbConnectionCsm, ILogger<ImportNotaRisikoQueryHandler> logger,
            IProfilePictureHelper pictureHelper, IConfiguration configuration)
        {
            _dbConnectionCsm = dbConnectionCsm;
            _logger = logger;
            _pictureHelper = pictureHelper;
            _configuration = configuration;
        }
    
        public async Task<Unit> Handle(ImportNotaRisikoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var asumsiPath = _configuration.GetSection("NotaRisiko").Value.TrimEnd('/');
                
                var path = await _pictureHelper.UploadToFolder(request.File, asumsiPath);
                
                await _dbConnectionCsm.QueryProc("sp_UploadNotaRisiko",
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