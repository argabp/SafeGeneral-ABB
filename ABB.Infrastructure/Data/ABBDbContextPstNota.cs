using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using ABB.Domain.IdentityModels;
using ABB.Infrastructure.Data.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ABB.Infrastructure.Data
{
    public class ABBDbContextPstNota : IdentityDbContext<AppUser, AppRole, string>, IDbContextPstNota
    {
        public ABBDbContextPstNota(DbContextOptions<ABBDbContextPstNota> options) : base(options)
        {
            DatabaseContext = Database;
        }

        public DatabaseFacade DatabaseContext { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}

