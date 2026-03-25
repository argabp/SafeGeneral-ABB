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
        public DbSet<DLAReasuransi> DLAReasuransi { get; set; }
        public DbSet<PLAReasuransi> PLAReasuransi { get; set; }
        public DbSet<NotaKlaimTreaty> NotaKlaimTreaty { get; set; }
        public DbSet<NotaKlaimReasuransi> NotaKlaimReasuransi { get; set; }
        public DbSet<KontrakTreatyMasuk> KontrakTreatyMasuk { get; set; }
        public DbSet<KontrakTreatyKeluarXOL> KontrakTreatyKeluarXOL { get; set; }
        public DbSet<DetailKontrakTreatyKeluarXOL> DetailKontrakTreatyKeluarXOL { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new KlaimAlokasiReasuransiMap());
            builder.ApplyConfiguration(new KlaimAlokasiReasuransiXLMap());
            builder.ApplyConfiguration(new DLAReasuransiMap());
            builder.ApplyConfiguration(new PLAReasuransiMap());
            builder.ApplyConfiguration(new NotaKlaimTreatyMap());
            builder.ApplyConfiguration(new NotaKlaimReasuransiMap());
            builder.ApplyConfiguration(new KontrakTreatyMasukMap());
            builder.ApplyConfiguration(new KontrakTreatyKeluarXOLMap());
            builder.ApplyConfiguration(new DetailKontrakTreatyKeluarXOLMap());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}

