using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ABB.Application.PengajuanAkseptasi.Commands
{
    public class ApprovalPengajuanAkseptasiCommand : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_aks { get; set; }

        public Int16 kd_status { get; set; }

        public string keterangan { get; set; }

        public List<IFormFile> Files { get; set; }
    }

    public class ApprovalPengajuanAkseptasiCommandHandler : IRequestHandler<ApprovalPengajuanAkseptasiCommand, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDbConnection _dbConnection;
        private readonly IConfiguration _configuration;
        private readonly IProfilePictureHelper _pictureHelper;
        private readonly ICurrentUserService _userService;

        public ApprovalPengajuanAkseptasiCommandHandler(IDbConnectionFactory connectionFactory, IDbConnection dbConnection,
            IConfiguration configuration, IProfilePictureHelper pictureHelper, ICurrentUserService userService)
        {
            _connectionFactory = connectionFactory;
            _dbConnection = dbConnection;
            _configuration = configuration;
            _pictureHelper = pictureHelper;
            _userService = userService;
        }

        public async Task<string> Handle(ApprovalPengajuanAkseptasiCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var message = string.Empty;
            try
            {
                message = (await _dbConnection.QueryProc<string>("sp_ApprovalPengajuanAks",
                    new
                    {
                        request.kd_cb, request.kd_cob, request.kd_scob,
                        request.kd_thn, request.no_aks, kd_user_status = _userService.UserId,
                        request.kd_status, tgl_status = DateTime.Now, request.keterangan
                    })).First();

                var no_urut = ( await _dbConnection.Query<string>($@"SELECT no_urut FROM TR_AkseptasiStatus WHERE 
                                                                                kd_cb = {request.kd_cb} AND kd_cob = {request.kd_cob} 
                                                                                AND kd_scob = {request.kd_scob} AND kd_thn = {request.kd_thn} AND
                                                                                kd_thn = {request.kd_thn} Order by no_urut desc")).First();
                
                var path = _configuration.GetSection("PengajuanAkseptasiStatusAttachment").Value.TrimEnd('/');
                var pengajuan = $@"{request.kd_cb.Trim()}{request.kd_cob.Trim()}{request.kd_scob.Trim()}{request.kd_thn}{request.no_aks}{no_urut}";
                path = Path.Combine(path, pengajuan.Replace("/", string.Empty));

                foreach (var file in request.Files)
                {
                    await _pictureHelper.UploadToFolder(file, path);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return message;
        }
    }
}