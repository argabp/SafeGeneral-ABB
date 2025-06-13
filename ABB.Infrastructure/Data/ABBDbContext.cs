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
    public class ABBDbContext : IdentityDbContext<AppUser, AppRole, string>, IDbContext
    {
        public ABBDbContext(DbContextOptions<ABBDbContext> options) : base(options)
        {
            DatabaseContext = Database;
        }

        public DatabaseFacade DatabaseContext { get; set; }
        public DbSet<AppRole> Role { get; set; }
        public DbSet<AppUser> User { get; set; }
        public DbSet<ApiAuthorization> ApiAuthorization { get; set; }
        public DbSet<UserHistory> UserHistory { get; set; }
        public DbSet<AuditTrail> AuditTrail { get; set; }
        public DbSet<Route> Route { get; set; }
        public DbSet<RoleLevel> RoleLevel { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Navigation> Navigation { get; set; }
        public DbSet<RoleNavigation> RoleNavigation { get; set; }
        public DbSet<RoleRoute> RoleRoute { get; set; }
        public DbSet<UserCabang> UserCabang { get; set; }
        public DbSet<Module> Module { get; set; }
        public DbSet<Cabang> Cabang { get; set; }
        public DbSet<RoleModule> RoleModule { get; set; }
        public DbSet<ModuleNavigation> ModuleNavigation { get; set; }
        public DbSet<COB> COB { get; set; }
        public DbSet<SCOB> SCOB { get; set; }
        public DbSet<MataUang> MataUang { get; set; }
        public DbSet<DetailMataUang> DetailMataUang { get; set; }
        public DbSet<Provinsi> Provinsi { get; set; }
        public DbSet<Kota> Kota { get; set; }
        public DbSet<Kabupaten> Kabupaten { get; set; }
        public DbSet<Kecamatan> Kecamatan { get; set; }
        public DbSet<Kelurahan> Kelurahan { get; set; }
        public DbSet<LokasiResiko> LokasiResiko { get; set; }
        public DbSet<DetailLokasiResiko> DetailLokasiResiko { get; set; }
        public DbSet<GrupResiko> GrupResiko { get; set; }
        public DbSet<DetailGrupResiko> DetailGrupResiko { get; set; }
        public DbSet<Okupasi> Okupasi { get; set; }
        public DbSet<DetailOkupasi> DetailOkupasi { get; set; }
        public DbSet<KelasKonstruksi> KelasKonstruksi { get; set; }
        public DbSet<GrupObyek> GrupObyek { get; set; }
        public DbSet<Zona> Zona { get; set; }
        public DbSet<DetailZona> DetailZona { get; set; }
        public DbSet<Rekanan> Rekanan { get; set; }
        public DbSet<DetailRekanan> DetailRekanan { get; set; }
        public DbSet<PeruntukanKendaraan> PeruntukanKendaraan { get; set; }
        public DbSet<Coverage> Coverage { get; set; }
        public DbSet<JenisSor> JenisSor { get; set; }
        public DbSet<Kapal> Kapal { get; set; }
        public DbSet<SebabKejadian> SebabKejadian { get; set; }
        public DbSet<DokumenKlaim> DokumenKlaim { get; set; }
        public DbSet<PertanggunganKendaraan> PertanggunganKendaraan { get; set; }
        public DbSet<DetailPertanggunganKendaraan> DetailPertanggunganKendaraan { get; set; }
        public DbSet<BiayaMaterai> BiayaMaterai { get; set; }
        public DbSet<BiayaPerSubCOB> BiayaPerSubCOB { get; set; }
        public DbSet<Akuisisi> Akuisisi { get; set; }
        public DbSet<LevelOtoritas> LevelOtoritas { get; set; }
        public DbSet<KapasitasCabang> KapasitasCabang { get; set; }
        public DbSet<RiskAndLossProfile> RiskAndLossProfile { get; set; }
        public DbSet<LimitTreaty> LimitTreaty { get; set; }
        public DbSet<KendaraanOJK> KendaraanOJK { get; set; }
        public DbSet<PolisInduk> PolisInduk { get; set; }
        public DbSet<Obligee> Obligee { get; set; }
        public DbSet<DetailObligee> DetailObligee { get; set; }
        public DbSet<KodeKonfirmasi> KodeKonfirmasi { get; set; }
        public DbSet<Akseptasi> Akseptasi { get; set; }
        public DbSet<AkseptasiResiko> AkseptasiResiko { get; set; }
        public DbSet<AkseptasiCoverage> AkseptasiCoverage { get; set; }
        public DbSet<AkseptasiObyek> AkseptasiObyek { get; set; }
        public DbSet<AkseptasiOtherFire> AkseptasiOtherFire { get; set; }
        public DbSet<AkseptasiOtherMotor> AkseptasiOtherMotor { get; set; }
        public DbSet<AkseptasiOtherMotorDetail> AkseptasiOtherMotorDetail { get; set; }
        public DbSet<AkseptasiOtherCargo> AkseptasiOtherCargo { get; set; }
        public DbSet<AkseptasiOtherCargoDetail> AkseptasiOtherCargoDetail { get; set; }
        public DbSet<AkseptasiOtherBonding> AkseptasiOtherBonding { get; set; }
        public DbSet<AkseptasiOtherPA> AkseptasiOtherPA { get; set; }
        public DbSet<AkseptasiOtherHull> AkseptasiOtherHull { get; set; }
        public DbSet<AkseptasiOtherHoleInOne> AkseptasiOtherHoleInOne { get; set; }
        public DbSet<AkseptasiPranota> AkseptasiPranota { get; set; }
        public DbSet<AkseptasiPranotaKoas> AkseptasiPranotaKoas { get; set; }
        public DbSet<Nota> Nota { get; set; }
        public DbSet<DetailNota> DetailNota { get; set; }
        public DbSet<DetailAlokasi> DetailAlokasi { get; set; }
        public DbSet<NotaKomisiTambahan> NotaKomisiTambahan { get; set; }
        public DbSet<NomorRegistrasiPolis> NomorRegistrasiPolis { get; set; }
        public DbSet<CopyEndors> CopyEndors { get; set; }
        public DbSet<TRAkseptasi> TRAkseptasi { get; set; }
        public DbSet<TRAkseptasiAttachment> TRAkseptasiAttachment { get; set; }
        public DbSet<TRAkseptasiStatus> TRAkseptasiStatus { get; set; }
        public DbSet<TRAkseptasiStatusAttachment> TRAkseptasiStatusAttachment { get; set; }
        public DbSet<Approval> Approval { get; set; }
        public DbSet<ApprovalDetail> ApprovalDetail { get; set; }
        public DbSet<EmailTemplate> EmailTemplate { get; set; }
        public DbSet<ViewTRAkseptasi> ViewTRAkseptasi { get; set; }
        public DbSet<AkseptasiProduk> AkseptasiProduk { get; set; }
        public DbSet<Asumsi> Asumsi { get; set; }
        public DbSet<AsumsiDetail> AsumsiDetail { get; set; }
        public DbSet<AsumsiPeriode> AsumsiPeriode { get; set; }
        public DbSet<PeriodeProsesModel> PeriodeProses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUserRole<string>>().ToTable("TR_UserRole", "dbo");
            builder.Entity<IdentityUserClaim<string>>().ToTable("MS_UserClaim", "dbo");
            builder.Entity<IdentityUserToken<string>>().ToTable("MS_UserToken", "dbo");
            builder.Entity<IdentityUserLogin<string>>().ToTable("MS_UserLogin", "dbo");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("MS_RoleClaim", "dbo");

            // Master
            builder.ApplyConfiguration(new UserMap());
            builder.ApplyConfiguration(new RoleMap());
            builder.ApplyConfiguration(new ApiAuthorizationMap());
            builder.ApplyConfiguration(new UserHistoryMap());
            builder.ApplyConfiguration(new AuditTrailMap());
            builder.ApplyConfiguration(new RouteMap());
            builder.ApplyConfiguration(new RoleLevelMap());
            builder.ApplyConfiguration(new NavigationMap());
            builder.ApplyConfiguration(new RoleNavigationMap());
            builder.ApplyConfiguration(new RoleRouteMap());
            builder.ApplyConfiguration(new ModuleMap());
            builder.ApplyConfiguration(new UserCabangMap());
            builder.ApplyConfiguration(new CabangMap());
            builder.ApplyConfiguration(new RoleModuleMap());
            builder.ApplyConfiguration(new ModuleNavigationMap());
            builder.ApplyConfiguration(new COBMap());
            builder.ApplyConfiguration(new SCOBMap());
            builder.ApplyConfiguration(new MataUangMap());
            builder.ApplyConfiguration(new DetailMataUangMap());
            builder.ApplyConfiguration(new ProvinsiMap());
            builder.ApplyConfiguration(new KotaMap());
            builder.ApplyConfiguration(new KabupatenMap());
            builder.ApplyConfiguration(new KecamatanMap());
            builder.ApplyConfiguration(new KelurahanMap());
            builder.ApplyConfiguration(new LokasiResikoMap());
            builder.ApplyConfiguration(new DetailLokasiResikoMap());
            builder.ApplyConfiguration(new GrupResikoMap());
            builder.ApplyConfiguration(new DetailGrupResikoMap());
            builder.ApplyConfiguration(new OkupasiMap());
            builder.ApplyConfiguration(new DetailOkupasiMap());
            builder.ApplyConfiguration(new KelasKonstruksiMap());
            builder.ApplyConfiguration(new GrupObyekMap());
            builder.ApplyConfiguration(new ZonaMap());
            builder.ApplyConfiguration(new DetailZonaMap());
            builder.ApplyConfiguration(new RekananMap());
            builder.ApplyConfiguration(new DetailRekananMap());
            builder.ApplyConfiguration(new PeruntukanKendaraanMap());
            builder.ApplyConfiguration(new CoverageMap());
            builder.ApplyConfiguration(new JenisSorMap());
            builder.ApplyConfiguration(new KapalMap());
            builder.ApplyConfiguration(new KapalMap());
            builder.ApplyConfiguration(new SebabKejadianMap());
            builder.ApplyConfiguration(new DokumenKlaimMap());
            builder.ApplyConfiguration(new PertanggunganKendaraanMap());
            builder.ApplyConfiguration(new DetailPertanggunganKendaraanMap());
            builder.ApplyConfiguration(new BiayaMateraiMap());
            builder.ApplyConfiguration(new BiayaPerSubCOBMap());
            builder.ApplyConfiguration(new AkuisisiMap());
            builder.ApplyConfiguration(new LevelOtoritasMap());
            builder.ApplyConfiguration(new KapasitasCabangMap());
            builder.ApplyConfiguration(new RiskAndLossProfileMap());
            builder.ApplyConfiguration(new LimitTreatyMap());
            builder.ApplyConfiguration(new KendaraanOJKMap());
            builder.ApplyConfiguration(new PolisIndukMap());
            builder.ApplyConfiguration(new ObligeeMap());
            builder.ApplyConfiguration(new DetailObligeeMap());
            builder.ApplyConfiguration(new KodeKonfirmasiMap());
            builder.ApplyConfiguration(new AkseptasiMap());
            builder.ApplyConfiguration(new AkseptasiResikoMap());
            builder.ApplyConfiguration(new AkseptasiCoverageMap());
            builder.ApplyConfiguration(new AkseptasiObyekMap());
            builder.ApplyConfiguration(new AkseptasiOtherFireMap());
            builder.ApplyConfiguration(new AkseptasiOtherMotorMap());
            builder.ApplyConfiguration(new AkseptasiOtherMotorDetailMap());
            builder.ApplyConfiguration(new AkseptasiOtherCargoMap());
            builder.ApplyConfiguration(new AkseptasiOtherCargoDetailMap());
            builder.ApplyConfiguration(new AkseptasiOtherBondingMap());
            builder.ApplyConfiguration(new AkseptasiOtherPAMap());
            builder.ApplyConfiguration(new AkseptasiOtherHullMap());
            builder.ApplyConfiguration(new AkseptasiOtherHoleInOneMap());
            builder.ApplyConfiguration(new AkseptasiPranotaMap());
            builder.ApplyConfiguration(new AkseptasiPranotaKoasMap());
            builder.ApplyConfiguration(new NotaMap());
            builder.ApplyConfiguration(new DetailNotaMap());
            builder.ApplyConfiguration(new DetailAlokasiMap());
            builder.ApplyConfiguration(new NotaKomisiTambahanMap());
            builder.ApplyConfiguration(new NomorRegistrasiPolisMap());
            builder.ApplyConfiguration(new CopyEndorsMap());
            builder.ApplyConfiguration(new TRAkseptasiMap());
            builder.ApplyConfiguration(new TRAkseptasiStatusMap());
            builder.ApplyConfiguration(new TRAkseptasiStatusAttachmentMap());
            builder.ApplyConfiguration(new TRAkseptasiAttachmentMap());
            builder.ApplyConfiguration(new ApprovalMap());
            builder.ApplyConfiguration(new ApprovalDetailMap());
            builder.ApplyConfiguration(new EmailTemplateMap());
            builder.ApplyConfiguration(new ViewTRAkseptasiMap());
            builder.ApplyConfiguration(new AkseptasiProdukMap());
            builder.ApplyConfiguration(new AsumsiMap());
            builder.ApplyConfiguration(new AsumsiDetailMap());
            builder.ApplyConfiguration(new AsumsiPeriodeMap());
            builder.ApplyConfiguration(new PeriodeProsesMap());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}

