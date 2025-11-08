using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ABB.Application.RegisterKlaims.Commands
{
    public class DeleteDokumenRegisterKlaimCommand : IRequest
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }
        
        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public string kd_dok { get; set; }
    }

    public class DeleteDokumenRegisterKlaimCommandHandler : IRequestHandler<DeleteDokumenRegisterKlaimCommand>
    {
        private readonly IDbContextFactory _context;
        private readonly ILogger<DeleteDokumenRegisterKlaimCommandHandler> _logger;
        private readonly IHostEnvironment _root;
        private readonly IConfiguration _config;

        public DeleteDokumenRegisterKlaimCommandHandler(IDbContextFactory context, ILogger<DeleteDokumenRegisterKlaimCommandHandler> logger,
                                                    IHostEnvironment root, IConfiguration config)
        {
            _context = context;
            _logger = logger;
            _root = root;
            _config = config;
        }

        public async Task<Unit> Handle(DeleteDokumenRegisterKlaimCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var context = _context.CreateDbContext(request.DatabaseName);
                var dokumenRegisterKlaim = context.DokumenRegisterKlaim.FirstOrDefault(w => w.kd_cb == request.kd_cb &&
                    w.kd_cob == request.kd_cob
                    && w.kd_scob == request.kd_scob &&
                    w.kd_thn == request.kd_thn
                    && w.no_kl == request.no_kl
                    && w.kd_dok == request.kd_dok);

                if (dokumenRegisterKlaim != null)
                {
                    context.DokumenRegisterKlaim.Remove(dokumenRegisterKlaim);

                    await context.SaveChangesAsync(cancellationToken);

                    if(!string.IsNullOrWhiteSpace(dokumenRegisterKlaim.link_file))
                    {
                        var registerKlaimPath =
                            $"{dokumenRegisterKlaim.kd_cb.Trim()}{dokumenRegisterKlaim.kd_cob.Trim()}{dokumenRegisterKlaim.kd_scob.Trim()}{dokumenRegisterKlaim.kd_thn.Trim()}{dokumenRegisterKlaim.no_kl.Trim()}";
                        DeleteAttachment(dokumenRegisterKlaim.link_file, registerKlaimPath);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }

            return Unit.Value;
        }

        public void DeleteAttachment(string filename, string nomor_pengjuan)
        {
            var path = _config.GetSection("DokumenRegisterKlaim").Value.TrimEnd('/');
            var folderPath = Path.Combine(path, nomor_pengjuan.Replace("/", string.Empty));
            
            var wwwroot = Path.Combine(_root.ContentRootPath, "wwwroot");
            var root = Path.Combine(wwwroot, folderPath.TrimStart('/'));
            var imgPath = Path.Combine(root, filename);

            var fi = new FileInfo(imgPath);
            if (fi.Exists)
            {
                fi.Delete();
            }
        }
    }
}