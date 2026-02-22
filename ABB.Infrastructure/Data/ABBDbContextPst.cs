using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using ABB.Domain.IdentityModels;
using ABB.Infrastructure.Data.Mapping;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ABB.Infrastructure.Data
{
    public class ABBDbContextPst : IdentityDbContext<AppUser, AppRole, string>, IDbContextPst
    {
        
        public ABBDbContextPst(DbContextOptions<ABBDbContextPst> options) : base(options)
        {
            DatabaseContext = Database;
        }

        public DatabaseFacade DatabaseContext { get; set; }
        public DbSet<KlaimAlokasiReasuransi> KlaimAlokasiReasuransi { get; set; }
        public DbSet<KlaimAlokasiReasuransiXL> KlaimAlokasiReasuransiXL { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new KlaimAlokasiReasuransiMap());
            builder.ApplyConfiguration(new KlaimAlokasiReasuransiXLMap());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}

