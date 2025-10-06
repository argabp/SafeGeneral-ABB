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

// alias
using KasBankEntity = ABB.Domain.Entities.KasBank;
using VoucherKasEntity = ABB.Domain.Entities.VoucherKas;
using VoucherBankEntity = ABB.Domain.Entities.VoucherBank;
using EntriPembayaranKasEntity = ABB.Domain.Entities.EntriPembayaranKas;
using EntriPembayaranBankEntity = ABB.Domain.Entities.EntriPembayaranBank;
using EntriPenyelesaianPiutangEntity = ABB.Domain.Entities.EntriPenyelesaianPiutang;
using HeaderPenyelesaianPiutangEntity = ABB.Domain.Entities.HeaderPenyelesaianUtang;
using MataUangEntity = ABB.Domain.Entities.MataUang;

namespace ABB.Infrastructure.Data
{
    public class ABBDbContextPstNota : IdentityDbContext<AppUser, AppRole, string>, IDbContextPstNota
    {
        public ABBDbContextPstNota(DbContextOptions<ABBDbContextPstNota> options) : base(options)
        {
            DatabaseContext = Database;
        }

        public DatabaseFacade DatabaseContext { get; set; }
        // tambahan kasbank
        public DbSet<KasBankEntity> KasBank { get; set; }
        public DbSet<VoucherKasEntity> VoucherKas { get; set; }
        public DbSet<VoucherBankEntity> VoucherBank { get; set; }
        public DbSet<EntriPembayaranKasEntity> EntriPembayaranKas { get; set; }
        public DbSet<EntriPembayaranBank> EntriPembayaranBank { get; set; }

        public DbSet<Produksi> Produksi { get; set; }
        public DbSet<Coa> Coa { get; set; }
        public DbSet<EntriPenyelesaianPiutangEntity> EntriPenyelesaianPiutang { get; set; }
        public DbSet<HeaderPenyelesaianPiutangEntity> HeaderPenyelesaianUtang { get; set; }
        public DbSet<MataUangEntity> MataUang { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // tambah mapping
            builder.ApplyConfiguration(new KasBankMap());
            builder.ApplyConfiguration(new VoucherKasMap());
            builder.ApplyConfiguration(new VoucherBankMap());
            builder.ApplyConfiguration(new EntriPembayaranKasMap());
            builder.ApplyConfiguration(new EntriPembayaranBankMap());
            builder.ApplyConfiguration(new ProduksiMap());
            builder.ApplyConfiguration(new CoaMap());
            builder.ApplyConfiguration(new EntriPenyelesaianPiutangMap());
            builder.ApplyConfiguration(new HeaderPenyelesaianUtangMap());
            builder.ApplyConfiguration(new MataUangMap());

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}

