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

namespace ABB.Application.UpdateKlaims.Commands
{
    public class UpdateKlaimCommand : IRequest<(string, List<string>)>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }

        public Int16 kd_status { get; set; }

        public string keterangan { get; set; }
    }

    public class UpdateKlaimCommandHandler : IRequestHandler<UpdateKlaimCommand, (string, List<string>)>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ICurrentUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IDbContext _dbContext;

        public UpdateKlaimCommandHandler(IDbConnectionFactory connectionFactory, ICurrentUserService userService, 
            IEmailService emailService, IDbContextFactory dbContextFactory, IDbContext dbContext)
        {
            _connectionFactory = connectionFactory;
            _userService = userService;
            _emailService = emailService;
            _dbContextFactory = dbContextFactory;
            _dbContext = dbContext;
        }

        public async Task<(string, List<string>)> Handle(UpdateKlaimCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var message = string.Empty;
            var userIds = new List<string>();
            try
            {
                    message = (await _connectionFactory.QueryProc<string>("sp_ApprovalPengajuanKlaimRCF",
                    new
                    {
                        request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn, 
                        request.no_kl, no_mts = 2, kd_user_status = _userService.UserId,
                        request.kd_status, tgl_status = DateTime.Now, request.keterangan,
                        kd_user_sign1 = _userService.UserId
                    })).First();

                var dbContext = _dbContextFactory.CreateDbContext(request.DatabaseName);
                

                var viewKlaim = dbContext.ViewTrKlaim.FirstOrDefault(w => w.kd_cb == request.kd_cb &&
                                                                              w.kd_cob == request.kd_cob &&
                                                                              w.kd_scob == request.kd_scob &&
                                                                              w.kd_thn == request.kd_thn &&
                                                                              w.no_kl == request.no_kl &&
                                                                              w.no_mts == 2);

                if (viewKlaim == null)
                {
                    throw new NullReferenceException("Data v_tr_klaim tidak ditemukan.");
                }
                
                userIds = dbContext.KlaimStatus.Where(w => w.kd_cb == request.kd_cb &&
                                                                    w.kd_cob == request.kd_cob &&
                                                                    w.kd_scob == request.kd_scob &&
                                                                    w.kd_thn == request.kd_thn &&
                                                                    w.no_kl == request.no_kl &&
                                                                    w.no_mts == 2)
                    .Select(s => s.kd_user_sign).ToList();
                
                userIds.Add(viewKlaim.kd_usr_input);

                var emailSends = _dbContext.User.Where(w => userIds.Distinct().Contains(w.Id)).Select(s => s.Email).ToList();

                await _emailService.SendMutasiKlaimEmail(emailSends, viewKlaim);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return (message, userIds.Distinct().ToList());
        }
    }
}