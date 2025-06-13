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
        public string DatabaseName { get; set; }
    }
    
    public class ImportNotaRisikoQueryHandler : IRequestHandler<ImportNotaRisikoQuery>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<ImportNotaRisikoQueryHandler> _logger;
        private readonly IProfilePictureHelper _pictureHelper;
        private readonly IConfiguration _configuration;

        public ImportNotaRisikoQueryHandler(IDbConnectionFactory dbConnectionFactory, ILogger<ImportNotaRisikoQueryHandler> logger,
            IProfilePictureHelper pictureHelper, IConfiguration configuration)
        {
            _dbConnectionFactory = dbConnectionFactory;
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
                
                _dbConnectionFactory.CreateDbConnection(request.DatabaseName);
                await _dbConnectionFactory.QueryProc("sp_UploadNotaRisiko",
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