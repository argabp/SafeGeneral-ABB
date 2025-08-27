using System.Threading;
using System.Threading.Tasks;
using ABB.Domain.Entities;
using ABB.Domain.IdentityModels;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.Common.Interfaces
{
    public interface IDbContextPstNota
    {
        DbSet<T> Set<T>() where T : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}