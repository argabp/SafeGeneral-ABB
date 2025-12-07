using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Scriban;

namespace ABB.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;
        private readonly IMediator _mediator;
        private readonly IDbContext _dbContext;

        public EmailService(IConfiguration config, ILogger<EmailService> logger, IMediator mediator, IDbContext dbContext)
        {
            _config = config;
            _logger = logger;
            _mediator = mediator;
            _dbContext = dbContext;
        }
        
        public async Task SendApprovalEmail(List<string> emailSends, ViewTRAkseptasi? viewTrAkseptasi)
        {
            var username = _config.GetSection("Email").GetSection("Username").Value;
            var password = _config.GetSection("Email").GetSection("Password").Value;
            var smtpServer = _config.GetSection("Email").GetSection("SMTP").Value;
            var port = _config.GetSection("Email").GetSection("Port").Value;
            var sendEmail = _config.GetSection("Email").GetSection("SendEmail").Value;
            
            if(!Convert.ToBoolean(sendEmail))
                return;

            ServicePointManager.Expect100Continue = false;
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            
            var smtpClient = new SmtpClient(smtpServer)
            {
                Port = Convert.ToInt32(port),
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true,
            };

            if (viewTrAkseptasi == null)
                throw new NullReferenceException("Akseptpasi not found");

            EmailTemplate emailTemplate = _dbContext.EmailTemplate.FirstOrDefault(w =>
                w.Name == "Pengajuan Akseptasi");

            Template templateEmailForNotification = Template.Parse( emailTemplate.Body );
            
            string emailTemplateHtml = await templateEmailForNotification.RenderAsync( new
            {
                viewTrAkseptasi.user_status,
                viewTrAkseptasi.nm_cb,
                viewTrAkseptasi.nm_cob,
                viewTrAkseptasi.nm_scob,
                viewTrAkseptasi.nomor_pengajuan,
                tgl_pengajuan = viewTrAkseptasi.tgl_pengajuan.Value.ToString("dd MMM yyyy"),
                viewTrAkseptasi.nm_tertanggung,
                tgl_status = viewTrAkseptasi.tgl_status.Value.ToString("dd MMM yyyy"),
                viewTrAkseptasi.ket_status,
                viewTrAkseptasi.status
            } );

            foreach (var sentTo in emailSends)
            {
                var body = emailTemplateHtml;
                MailAddress from = new MailAddress(username);
                MailAddress to = new MailAddress(sentTo);

                MailMessage message = new MailMessage(from, to);
                message.Subject = $"Nomor Pengajuan Akseptasi {viewTrAkseptasi.nomor_pengajuan} {viewTrAkseptasi.status}";

                message.Body = body;
                message.IsBodyHtml = true;
                
                try
                {
                    smtpClient.Send(message);
                }
                catch (SmtpException ex)
                {
                    _logger.LogError(ex.Message);
                    throw;
                }
            }
        }
        
        public async Task SendMutasiKlaimEmail(List<string> emailSends, ViewTrKlaim? viewTrKlaim)
        {
            var username = _config.GetSection("Email").GetSection("Username").Value;
            var password = _config.GetSection("Email").GetSection("Password").Value;
            var smtpServer = _config.GetSection("Email").GetSection("SMTP").Value;
            var port = _config.GetSection("Email").GetSection("Port").Value;
            var sendEmail = _config.GetSection("Email").GetSection("SendEmail").Value;
            
            if(!Convert.ToBoolean(sendEmail))
                return;

            ServicePointManager.Expect100Continue = false;
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            
            var smtpClient = new SmtpClient(smtpServer)
            {
                Port = Convert.ToInt32(port),
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true,
            };

            if (viewTrKlaim == null)
                throw new NullReferenceException("Klaim not found");

            EmailTemplate emailTemplate = _dbContext.EmailTemplate.FirstOrDefault(w =>
                w.Name == "Pengajuan Klaim");

            Template templateEmailForNotification = Template.Parse( emailTemplate.Body );
            
            string emailTemplateHtml = await templateEmailForNotification.RenderAsync( new
            {
                viewTrKlaim.user_status,
                viewTrKlaim.nm_cb,
                viewTrKlaim.nm_cob,
                viewTrKlaim.nm_scob,
                viewTrKlaim.nomor_berkas,
                tgl_reg = viewTrKlaim.tgl_reg.Value.ToString("dd MMM yyyy"),
                viewTrKlaim.nm_tertanggung,
                tgl_status = viewTrKlaim.tgl_status.Value.ToString("dd MMM yyyy"),
                viewTrKlaim.ket_status,
                viewTrKlaim.status
            } );

            foreach (var sentTo in emailSends)
            {
                var body = emailTemplateHtml;
                MailAddress from = new MailAddress(username);
                MailAddress to = new MailAddress(sentTo);

                MailMessage message = new MailMessage(from, to);
                message.Subject = $"Nomor Pengajuan Klaim {viewTrKlaim.nomor_berkas} {viewTrKlaim.status}";

                message.Body = body;
                message.IsBodyHtml = true;
                message.Subject = $"Nomor Pengajuan Akseptasi {viewTrKlaim.nomor_berkas} {viewTrKlaim.status}";
                
                try
                {
                    smtpClient.Send(message);
                }
                catch (SmtpException ex)
                {
                    _logger.LogError(ex.Message);
                    throw;
                }
            }
        }
    }
}