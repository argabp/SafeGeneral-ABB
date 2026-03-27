using System.Threading;
using System.Threading.Tasks;
using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ABB.Application.Common.Interfaces
{
    public interface IDbContextPst
    {
        DatabaseFacade Database { get; }
        DbSet<T> Set<T>() where T : class;
        DbSet<KlaimAlokasiReasuransi> KlaimAlokasiReasuransi { get; set; }
        DbSet<KlaimAlokasiReasuransiXL> KlaimAlokasiReasuransiXL { get; set; }
        DbSet<DLAReasuransi> DLAReasuransi { get; set; }
        DbSet<PLAReasuransi> PLAReasuransi { get; set; }
        DbSet<NotaKlaimTreaty> NotaKlaimTreaty { get; set; }
        DbSet<NotaKlaimReasuransi> NotaKlaimReasuransi { get; set; }
        DbSet<KontrakTreatyMasuk> KontrakTreatyMasuk { get; set; }
        DbSet<KontrakTreatyKeluarXOL> KontrakTreatyKeluarXOL { get; set; }
        DbSet<DetailKontrakTreatyKeluarXOL> DetailKontrakTreatyKeluarXOL { get; set; }
        DbSet<KontrakTreatyKeluar> KontrakTreatyKeluar { get; set; }
        DbSet<DetailKontrakTreatyKeluar> DetailKontrakTreatyKeluar { get; set; }
        DbSet<DetailKontrakTreatyKeluarSCOB> DetailKontrakTreatyKeluarSCOB { get; set; }
        DbSet<DetailKontrakTreatyKeluarExclude> DetailKontrakTreatyKeluarExclude { get; set; }
        DbSet<DetailKontrakTreatyKeluarTableOfLimit> DetailKontrakTreatyKeluarTableOfLimit { get; set; }
        DbSet<DetailKontrakTreatyKeluarKoasuransi> DetailKontrakTreatyKeluarKoasuransi { get; set; }
        DbSet<DetailKontrakTreatyKeluarCoverage> DetailKontrakTreatyKeluarCoverage { get; set; }
        DbSet<NotaTreatyMasuk> NotaTreatyMasuk { get; set; }
        DbSet<TransaksiTreatyMasuk> TransaksiTreatyMasuk { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}