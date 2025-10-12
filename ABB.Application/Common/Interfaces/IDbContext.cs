using System.Threading;
using System.Threading.Tasks;
using ABB.Domain.Entities;
using ABB.Domain.IdentityModels;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.Common.Interfaces
{
    public interface IDbContext
    {
        DbSet<T> Set<T>() where T : class;
        DbSet<AppUser> User { get; set; }
        DbSet<AppRole> Role { get; set; }
        DbSet<ApiAuthorization> ApiAuthorization { get; set; }
        DbSet<UserHistory> UserHistory { get; set; }
        DbSet<AuditTrail> AuditTrail { get; set; }
        DbSet<Route> Route { get; set; }
        DbSet<RoleLevel> RoleLevel { get; set; }
        DbSet<UserRole> UserRole { get; set; }
        DbSet<Navigation> Navigation { get; set; }
        DbSet<RoleNavigation> RoleNavigation { get; set; }
        DbSet<RoleRoute> RoleRoute { get; set; }
        DbSet<UserCabang> UserCabang { get; set; }
        DbSet<Module> Module { get; set; }
        DbSet<Cabang> Cabang { get; set; }
        DbSet<RoleModule> RoleModule { get; set; }
        DbSet<ModuleNavigation> ModuleNavigation { get; set; }
        DbSet<COB> COB { get; set; }
        DbSet<SCOB> SCOB { get; set; }
        DbSet<MataUang> MataUang { get; set; }
        DbSet<DetailMataUang> DetailMataUang { get; set; }
        DbSet<Provinsi> Provinsi { get; set; }
        DbSet<Kota> Kota { get; set; }
        DbSet<Kabupaten> Kabupaten { get; set; }
        DbSet<Kecamatan> Kecamatan { get; set; }
        DbSet<Kelurahan> Kelurahan { get; set; }
        DbSet<LokasiResiko> LokasiResiko { get; set; }
        DbSet<DetailLokasiResiko> DetailLokasiResiko { get; set; }
        DbSet<GrupResiko> GrupResiko { get; set; }
        DbSet<DetailGrupResiko> DetailGrupResiko { get; set; }
        DbSet<Okupasi> Okupasi { get; set; }
        DbSet<DetailOkupasi> DetailOkupasi { get; set; }
        DbSet<KelasKonstruksi> KelasKonstruksi { get; set; }
        DbSet<GrupObyek> GrupObyek { get; set; }
        DbSet<Zona> Zona { get; set; }
        DbSet<DetailZona> DetailZona { get; set; }
        DbSet<Rekanan> Rekanan { get; set; }
        DbSet<DetailRekanan> DetailRekanan { get; set; }
        DbSet<PeruntukanKendaraan> PeruntukanKendaraan { get; set; }
        DbSet<Coverage> Coverage { get; set; }
        DbSet<JenisSor> JenisSor { get; set; }
        DbSet<Kapal> Kapal { get; set; }
        DbSet<SebabKejadian> SebabKejadian { get; set; }
        DbSet<DokumenKlaim> DokumenKlaim { get; set; }
        DbSet<PertanggunganKendaraan> PertanggunganKendaraan { get; set; }
        DbSet<DetailPertanggunganKendaraan> DetailPertanggunganKendaraan { get; set; }
        DbSet<BiayaMaterai> BiayaMaterai { get; set; }
        DbSet<BiayaPerSubCOB> BiayaPerSubCOB { get; set; }
        DbSet<Akuisisi> Akuisisi { get; set; }
        DbSet<LevelOtoritas> LevelOtoritas { get; set; }
        DbSet<KapasitasCabang> KapasitasCabang { get; set; }
        DbSet<RiskAndLossProfile> RiskAndLossProfile { get; set; }
        DbSet<LimitTreaty> LimitTreaty { get; set; }
        DbSet<KendaraanOJK> KendaraanOJK { get; set; }
        DbSet<PolisInduk> PolisInduk { get; set; }
        DbSet<Obligee> Obligee { get; set; }
        DbSet<DetailObligee> DetailObligee { get; set; }
        DbSet<KodeKonfirmasi> KodeKonfirmasi { get; set; }
        DbSet<Akseptasi> Akseptasi { get; set; }
        DbSet<AkseptasiResiko> AkseptasiResiko { get; set; }
        DbSet<AkseptasiCoverage> AkseptasiCoverage { get; set; }
        DbSet<AkseptasiObyek> AkseptasiObyek { get; set; }
        DbSet<AkseptasiObyekCIT> AkseptasiObyekCIT { get; set; }
        DbSet<AkseptasiObyekCIS> AkseptasiObyekCIS { get; set; }
        DbSet<AkseptasiOtherFire> AkseptasiOtherFire { get; set; }
        DbSet<AkseptasiOtherMotor> AkseptasiOtherMotor { get; set; }
        DbSet<AkseptasiOtherMotorDetail> AkseptasiOtherMotorDetail { get; set; }
        DbSet<AkseptasiOtherCargo> AkseptasiOtherCargo { get; set; }
        DbSet<AkseptasiOtherCargoDetail> AkseptasiOtherCargoDetail { get; set; }
        DbSet<AkseptasiOtherBonding> AkseptasiOtherBonding { get; set; }
        DbSet<AkseptasiOtherPA> AkseptasiOtherPA { get; set; }
        DbSet<AkseptasiOtherHull> AkseptasiOtherHull { get; set; }
        DbSet<AkseptasiOtherHoleInOne> AkseptasiOtherHoleInOne { get; set; }
        DbSet<AkseptasiPranota> AkseptasiPranota { get; set; }
        DbSet<AkseptasiPranotaKoas> AkseptasiPranotaKoas { get; set; }
        DbSet<Inquiry> Inquiry { get; set; }
        DbSet<InquiryResiko> InquiryResiko { get; set; }
        DbSet<InquiryCoverage> InquiryCoverage { get; set; }
        DbSet<InquiryObyek> InquiryObyek { get; set; }
        DbSet<InquiryObyekCIT> InquiryObyekCIT { get; set; }
        DbSet<InquiryObyekCIS> InquiryObyekCIS { get; set; }
        DbSet<InquiryOtherFire> InquiryOtherFire { get; set; }
        DbSet<InquiryOtherMotor> InquiryOtherMotor { get; set; }
        DbSet<InquiryOtherMotorDetail> InquiryOtherMotorDetail { get; set; }
        DbSet<InquiryOtherCargo> InquiryOtherCargo { get; set; }
        DbSet<InquiryOtherCargoDetail> InquiryOtherCargoDetail { get; set; }
        DbSet<InquiryOtherBonding> InquiryOtherBonding { get; set; }
        DbSet<InquiryOtherPA> InquiryOtherPA { get; set; }
        DbSet<InquiryOtherHull> InquiryOtherHull { get; set; }
        DbSet<InquiryOtherHoleInOne> InquiryOtherHoleInOne { get; set; }
        DbSet<InquiryPranota> InquiryPranota { get; set; }
        DbSet<InquiryPranotaKoas> InquiryPranotaKoas { get; set; }
        DbSet<Nota> Nota { get; set; }
        DbSet<DetailNota> DetailNota { get; set; }
        DbSet<DetailAlokasi> DetailAlokasi { get; set; }
        DbSet<NotaKomisiTambahan> NotaKomisiTambahan { get; set; }
        DbSet<Domain.Entities.NomorRegistrasiPolis> NomorRegistrasiPolis { get; set; }
        DbSet<CopyEndors> CopyEndors { get; set; }
        DbSet<TRAkseptasi> TRAkseptasi { get; set; }
        DbSet<TRAkseptasiAttachment> TRAkseptasiAttachment { get; set; }
        DbSet<TRAkseptasiStatus> TRAkseptasiStatus { get; set; }
        DbSet<TRAkseptasiStatusAttachment> TRAkseptasiStatusAttachment { get; set; }
        DbSet<Approval> Approval { get; set; }
        DbSet<ApprovalDetail> ApprovalDetail { get; set; }
        DbSet<EmailTemplate> EmailTemplate { get; set; }
        DbSet<ViewTRAkseptasi> ViewTRAkseptasi { get; set; }
        DbSet<AkseptasiProduk> AkseptasiProduk { get; set; }
        DbSet<DokumenDetil> DokumenDetil { get; set; }
        DbSet<DokumenAkseptasi> DokumenAkseptasi { get; set; }
        DbSet<DokumenAkseptasiDetil> DokumenAkseptasiDetil { get; set; }
        DbSet<LimitAkseptasi> LimitAkseptasi { get; set; }
        DbSet<LimitAkseptasiDetil> LimitAkseptasiDetil { get; set; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}