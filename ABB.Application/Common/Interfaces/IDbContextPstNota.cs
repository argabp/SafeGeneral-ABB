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
using EntriPenyelesaianPiutangEntity = ABB.Domain.Entities.EntriPenyelesaianPiutang;
using HeaderPenyelesaianPiutangEntity = ABB.Domain.Entities.HeaderPenyelesaianUtang;
using MataUangEntity = ABB.Domain.Entities.MataUang;


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
        DbSet<EntriPenyelesaianPiutangEntity> EntriPenyelesaianPiutang { get; set; }
        DbSet<HeaderPenyelesaianPiutangEntity> HeaderPenyelesaianUtang { get; set; }
        DbSet<MataUangEntity> MataUang { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}