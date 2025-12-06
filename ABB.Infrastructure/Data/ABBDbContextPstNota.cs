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
using CabangEntity = ABB.Domain.Entities.Cabang;
using TypeCoaEntity = ABB.Domain.Entities.TypeCoa;
using EntriPembayaranKasTempEntity = ABB.Domain.Entities.EntriPembayaranKasTemp;
using EntriPembayaranBankTempEntity = ABB.Domain.Entities.EntriPembayaranBankTemp;
using EntriPenyelesaianPiutangTempEntity = ABB.Domain.Entities.EntriPenyelesaianPiutangTemp;
using EntriPeriodeEntity = ABB.Domain.Entities.EntriPeriode;
using TipeAkun117Entity = ABB.Domain.Entities.TipeAkun117;
using TemplateJurnal62Entity = ABB.Domain.Entities.TemplateJurnal62;
using TemplateJurnalDetail62Entity = ABB.Domain.Entities.TemplateJurnalDetail62;

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
        public DbSet<Coa117> Coa117 { get; set; }
        public DbSet<EntriPenyelesaianPiutangEntity> EntriPenyelesaianPiutang { get; set; }
        public DbSet<HeaderPenyelesaianPiutangEntity> HeaderPenyelesaianUtang { get; set; }
        public DbSet<MataUangEntity> MataUang { get; set; }
        public DbSet<CabangEntity> Cabang { get; set; }
        public DbSet<TypeCoaEntity> TypeCoa { get; set; }
        public DbSet<EntriPembayaranKasTempEntity> EntriPembayaranKasTemp { get; set; }
        public DbSet<EntriPembayaranBankTempEntity> EntriPembayaranBankTemp { get; set; }
        public DbSet<EntriPenyelesaianPiutangTempEntity> EntriPenyelesaianPiutangTemp { get; set; }
        public DbSet<EntriPeriodeEntity> EntriPeriode { get; set; }
        public DbSet<TipeAkun117Entity> TipeAkun117 { get; set; }
        public DbSet<TemplateJurnal62Entity> TemplateJurnal62 { get; set; }
        public DbSet<TemplateJurnalDetail62Entity> TemplateJurnalDetail62 { get; set; }

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
            builder.ApplyConfiguration(new Coa117Map());
            builder.ApplyConfiguration(new EntriPenyelesaianPiutangMap());
            builder.ApplyConfiguration(new HeaderPenyelesaianUtangMap());
            builder.ApplyConfiguration(new MataUangMap());
            builder.ApplyConfiguration(new CabangMap());
            builder.ApplyConfiguration(new TypeCoaMap());
            builder.ApplyConfiguration(new EntriPembayaranKasTempMap());
            builder.ApplyConfiguration(new EntriPembayaranBankTempMap());
            builder.ApplyConfiguration(new EntriPenyelesaianPiutangTempMap());
            builder.ApplyConfiguration(new EntriPeriodeMap());
            builder.ApplyConfiguration(new TipeAkun117Map());
            builder.ApplyConfiguration(new TemplateJurnal62Map());
            builder.ApplyConfiguration(new TemplateJurnalDetail62Map());


        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}

