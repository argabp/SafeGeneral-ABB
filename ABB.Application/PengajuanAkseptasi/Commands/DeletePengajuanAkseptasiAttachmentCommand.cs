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

namespace ABB.Application.PengajuanAkseptasi.Commands
{
    public class DeletePengajuanAkseptasiAttachmentCommand : IRequest
    {
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }
        
        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 kd_jns_dokumen { get; set; }

        public Int16 kd_dokumen { get; set; }
    }

    public class DeletePengajuanAkseptasiAttachmentCommandHandler : IRequestHandler<DeletePengajuanAkseptasiAttachmentCommand>
    {
        private readonly IDbContext _context;
        private readonly ILogger<DeletePengajuanAkseptasiAttachmentCommandHandler> _logger;
        private readonly IHostEnvironment _root;
        private readonly IConfiguration _config;

        public DeletePengajuanAkseptasiAttachmentCommandHandler(IDbContext context, ILogger<DeletePengajuanAkseptasiAttachmentCommandHandler> logger,
                                                    IHostEnvironment root, IConfiguration config)
        {
            _context = context;
            _logger = logger;
            _root = root;
            _config = config;
        }

        public async Task<Unit> Handle(DeletePengajuanAkseptasiAttachmentCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var akseptasiAttachment = _context.TRAkseptasiAttachment.FirstOrDefault(w => w.kd_cb == request.kd_cb &&
                    w.kd_cob == request.kd_cob
                    && w.kd_scob == request.kd_scob &&
                    w.kd_thn == request.kd_thn
                    && w.no_aks == request.no_aks &&
                    w.kd_jns_dokumen == request.kd_jns_dokumen
                    && w.kd_dokumen == request.kd_dokumen);

                var akseptasi = _context.TRAkseptasi.FirstOrDefault(w => w.kd_cb == request.kd_cb &&
                                                                         w.kd_cob == request.kd_cob
                                                                         && w.kd_scob == request.kd_scob &&
                                                                         w.kd_thn == request.kd_thn
                                                                         && w.no_aks == request.no_aks);
                
                _context.TRAkseptasiAttachment.Remove(akseptasiAttachment);

                await _context.SaveChangesAsync(cancellationToken);

                DeleteAttachment(akseptasiAttachment.nm_dokumen, akseptasi.nomor_pengajuan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Unit.Value;
        }

        public void DeleteAttachment(string filename, string nomor_pengjuan)
        {
            var path = _config.GetSection("PengajuanAkseptasiAttachment").Value.TrimEnd('/');
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