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
    public class ABBDbContextCSM : IdentityDbContext<AppUser, AppRole, string>, IDbContextCSM
    {
        public ABBDbContextCSM(DbContextOptions<ABBDbContextCSM> options) : base(options)
        {
            DatabaseContext = Database;
        }

        public DatabaseFacade DatabaseContext { get; set; }
        public DbSet<Asumsi> Asumsi { get; set; }
        public DbSet<AsumsiDetail> AsumsiDetail { get; set; }
        public DbSet<AsumsiPeriode> AsumsiPeriode { get; set; }
        public DbSet<PeriodeProsesModel> PeriodeProses { get; set; }
        public DbSet<ViewSourceData> ViewSourceData { get; set; }
        public DbSet<ViewSourceDataCancel> ViewSourceDataCancel { get; set; }
        public DbSet<InitialPAA> InitialPAA { get; set; }
        public DbSet<SubsequentPAA> SubsequentPAA { get; set; }
        public DbSet<IntialLiability> IntialLiability { get; set; }
        public DbSet<IntialRelease> IntialRelease { get; set; }
        public DbSet<Subsequent> Subsequent { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.ApplyConfiguration(new AsumsiMap());
            builder.ApplyConfiguration(new AsumsiDetailMap());
            builder.ApplyConfiguration(new AsumsiPeriodeMap());
            builder.ApplyConfiguration(new PeriodeProsesMap());
            builder.ApplyConfiguration(new ViewSourceDataMap());
            builder.ApplyConfiguration(new ViewSourceDataCancelMap());
            builder.ApplyConfiguration(new InitialPAAMap());
            builder.ApplyConfiguration(new SubsequentPAAMap());
            builder.ApplyConfiguration(new IntialLiabilityMap());
            builder.ApplyConfiguration(new IntialReleaseMap());
            builder.ApplyConfiguration(new SubsequentMap());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}

