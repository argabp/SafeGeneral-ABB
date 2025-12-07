using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ABB.Application.MutasiKlaims.Commands
{
    public class ProcessMutasiKlaimCommand : IRequest<(string, List<string>)>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public Int16 no_mts { get; set; }

        public Int16 kd_status { get; set; }

        public string keterangan { get; set; }

        // public List<IFormFile> Files { get; set; }
    }

    public class ProcessMutasiKlaimCommandHandler : IRequestHandler<ProcessMutasiKlaimCommand, (string, List<string>)>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<ProcessMutasiKlaimCommandHandler> _logger;
        private readonly IConfiguration _configuration;
        private readonly IProfilePictureHelper _pictureHelper;
        private readonly ICurrentUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IDbContext _dbContext;

        public ProcessMutasiKlaimCommandHandler(IDbConnectionFactory connectionFactory, ILogger<ProcessMutasiKlaimCommandHandler> logger,
            IConfiguration configuration, IProfilePictureHelper pictureHelper, ICurrentUserService userService, 
            IEmailService emailService, IDbContextFactory dbContextFactory, IDbContext dbContext)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
            _configuration = configuration;
            _pictureHelper = pictureHelper;
            _userService = userService;
            _emailService = emailService;
            _dbContextFactory = dbContextFactory;
            _dbContext = dbContext;
        }

        public async Task<(string, List<string>)> Handle(ProcessMutasiKlaimCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var message = string.Empty;
            var userIds = new List<string>();
            try
            {
                    message = (await _connectionFactory.QueryProc<string>("sp_ApprovalPengajuanKlaim",
                    new
                    {
                        request.kd_cb, request.kd_cob, request.kd_scob,
                        request.kd_thn, request.no_kl, request.no_mts, kd_user_status = _userService.UserId,
                        request.kd_status, tgl_status = DateTime.Now, request.keterangan
                    })).First();

                // var no_urut = ( await _connectionFactory.Query<Int16>($@"SELECT no_urut FROM TR_KlaimStatus WHERE 
                //                                                                 kd_cb = '{request.kd_cb}' AND kd_cob = '{request.kd_cob}' 
                //                                                                 AND kd_scob = '{request.kd_scob}' AND kd_thn = '{request.kd_thn}' AND
                //                                                                 no_kl = '{request.no_kl}' AND no_mts = '{request.no_mts}' Order by no_urut desc")).First();
                
                // var path = _configuration.GetSection("PengajuanKlaimStatusAttachment").Value.TrimEnd('/');
                // var pengajuan = $@"{request.kd_cb.Trim()}{request.kd_cob.Trim()}{request.kd_scob.Trim()}{request.kd_thn}{request.no_kl}{request.no_mts}{no_urut}";
                // path = Path.Combine(path, pengajuan.Replace("/", string.Empty));
                // Int16 sequence = 0;
                // var statusAttachments = new List<KlaimStatusAttachment>();
                // foreach (var file in request.Files)
                // {
                //     await _pictureHelper.UploadToFolder(file, path);
                //
                //     sequence++;
                //
                //     statusAttachments.Add(new KlaimStatusAttachment()
                //     {
                //         no_urut = no_urut,
                //         kd_cb = request.kd_cb,
                //         kd_cob = request.kd_cob,
                //         kd_scob = request.kd_scob,
                //         kd_thn = request.kd_thn,
                //         no_kl = request.no_kl,
                //         no_mts = request.no_mts,
                //         no_dokumen = sequence,
                //         nm_dokumen = file.FileName
                //     });
                // }

                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);
                
                // dbContext.KlaimStatusAttachment.AddRange(statusAttachments);

                await dbContext.SaveChangesAsync(cancellationToken);

                var viewAkseptasi = dbContext.ViewTrKlaim.FirstOrDefault(w => w.kd_cb == request.kd_cb &&
                                                                              w.kd_cob == request.kd_cob &&
                                                                              w.kd_scob == request.kd_scob &&
                                                                              w.kd_thn == request.kd_thn &&
                                                                              w.no_kl == request.no_kl &&
                                                                              w.no_mts == request.no_mts);

                
                userIds = dbContext.KlaimStatus.Where(w => w.kd_cb == request.kd_cb &&
                                                                    w.kd_cob == request.kd_cob &&
                                                                    w.kd_scob == request.kd_scob &&
                                                                    w.kd_thn == request.kd_thn &&
                                                                    w.no_kl == request.no_kl &&
                                                                    w.no_mts == request.no_mts)
                    .Select(s => s.kd_user_sign).ToList();
                
                userIds.Add(viewAkseptasi.kd_usr_input);

                var emailSends = _dbContext.User.Where(w => userIds.Distinct().Contains(w.Id)).Select(s => s.Email).ToList();

                await _emailService.SendMutasiKlaimEmail(emailSends, viewAkseptasi);
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.Message ?? e.Message);
                throw;
            }

            return (message, userIds.Distinct().ToList());
        }
    }
}