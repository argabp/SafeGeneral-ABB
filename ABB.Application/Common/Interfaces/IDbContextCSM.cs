using System.Threading;
using System.Threading.Tasks;
using ABB.Domain.Entities;
using ABB.Domain.IdentityModels;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.Common.Interfaces
{
    public interface IDbContextCSM
    {
        DbSet<T> Set<T>() where T : class;
        DbSet<Asumsi> Asumsi { get; set; }
        DbSet<AsumsiDetail> AsumsiDetail { get; set; }
        DbSet<AsumsiPeriode> AsumsiPeriode { get; set; }
        DbSet<PeriodeProsesModel> PeriodeProses { get; set; }
        DbSet<ViewSourceData> ViewSourceData { get; set; }
        DbSet<ViewSourceDataCancel> ViewSourceDataCancel { get; set; }
        DbSet<InitialPAA> InitialPAA { get; set; }
        DbSet<SubsequentPAA> SubsequentPAA { get; set; }
        DbSet<IntialLiability> IntialLiability { get; set; }
        DbSet<IntialRelease> IntialRelease { get; set; }
        DbSet<Subsequent> Subsequent { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}