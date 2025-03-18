using System.Threading.Tasks;
using ABB.Application.Common.Dtos;

namespace ABB.Application.Common.Services
{
    public interface IAuditTrailService
    {
        Task Create(AuditTrailDto model);
    }
}