using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Domain.Entities;

namespace ABB.Application.Common.Services
{
    public interface IEmailService
    {
        Task SendApprovalEmail(List<string> emailSends, ViewTRAkseptasi? viewTrAkseptasi);
    }
}