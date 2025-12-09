using System.Threading;
using System.Threading.Tasks;
using ABB.Domain.Entities;
using ABB.Domain.IdentityModels;
using Microsoft.EntityFrameworkCore;

// alias
using KasBankEntity = ABB.Domain.Entities.KasBank; 
using VoucherKasEntity = ABB.Domain.Entities.VoucherKas;
using VoucherBankEntity = ABB.Domain.Entities.VoucherBank;
using EntriPembayaranKasEntity = ABB.Domain.Entities.EntriPembayaranKas;
using EntriPembayaranBankEntity = ABB.Domain.Entities.EntriPembayaranBank;
using ProduksiEntity = ABB.Domain.Entities.Produksi;
using CoaEntity = ABB.Domain.Entities.Coa;
using Coa117Entity = ABB.Domain.Entities.Coa117;
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
using TemplateJurnal117Entity = ABB.Domain.Entities.TemplateJurnal117;
using TemplateJurnalDetail117Entity = ABB.Domain.Entities.TemplateJurnalDetail117;


namespace ABB.Application.Common.Interfaces
{
    public interface IDbContextPstNota
    {
        DbSet<T> Set<T>() where T : class;
        DbSet<KasBankEntity> KasBank { get; set; }
        DbSet<VoucherKasEntity> VoucherKas { get; set; }
        DbSet<VoucherBankEntity> VoucherBank { get; set; }
        DbSet<EntriPembayaranKasEntity> EntriPembayaranKas { get; set; }
        DbSet<EntriPembayaranBankEntity> EntriPembayaranBank { get; set; }
        DbSet<ProduksiEntity> Produksi { get; set; }
        DbSet<CoaEntity> Coa { get; set; }
        DbSet<Coa117Entity> Coa117 { get; set; }
        DbSet<EntriPenyelesaianPiutangEntity> EntriPenyelesaianPiutang { get; set; }
        DbSet<HeaderPenyelesaianPiutangEntity> HeaderPenyelesaianUtang { get; set; }
        DbSet<MataUangEntity> MataUang { get; set; }
        DbSet<CabangEntity> Cabang { get; set; }
        DbSet<TypeCoaEntity> TypeCoa { get; set; }
        DbSet<EntriPembayaranKasTempEntity> EntriPembayaranKasTemp { get; set; }
        DbSet<EntriPembayaranBankTempEntity> EntriPembayaranBankTemp { get; set; }
        DbSet<EntriPenyelesaianPiutangTempEntity> EntriPenyelesaianPiutangTemp { get; set; }
        DbSet<EntriPeriodeEntity> EntriPeriode { get; set; }
        DbSet<TipeAkun117Entity> TipeAkun117 { get; set; }
        DbSet<TemplateJurnal62Entity> TemplateJurnal62 { get; set; }
        DbSet<TemplateJurnalDetail62Entity> TemplateJurnalDetail62 { get; set; }
        DbSet<TemplateJurnal117Entity> TemplateJurnal117 { get; set; }
        DbSet<TemplateJurnalDetail117Entity> TemplateJurnalDetail117 { get; set; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}