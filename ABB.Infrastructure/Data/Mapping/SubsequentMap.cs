using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABB.Infrastructure.Data.Mapping
{
    public class SubsequentMap : IEntityTypeConfiguration<Subsequent>
    {
        public void Configure(EntityTypeBuilder<Subsequent> builder)
        {
            builder.ToTable("TR_Subsequent", "dbo");
            builder.HasKey(k => new { k.Id, k.PeriodeProses });
            
            builder.Property(p => p.BELclaimliability).HasPrecision(21, 6);
            builder.Property(p => p.BELclaimRelease).HasPrecision(21, 6);
            builder.Property(p => p.UnwindingClaim).HasPrecision(21, 6);
            builder.Property(p => p.BELclaimCurrent).HasPrecision(21, 6);
            builder.Property(p => p.OCIBELclaim).HasPrecision(21, 6);
            builder.Property(p => p.BELexpenseLiability).HasPrecision(21, 6);
            builder.Property(p => p.BELexpenseRelease).HasPrecision(21, 6);
            builder.Property(p => p.UnwindingExpense).HasPrecision(21, 6);
            builder.Property(p => p.BELexpenseCurrent).HasPrecision(21, 6);
            builder.Property(p => p.OCIBELexpense).HasPrecision(21, 6);
            builder.Property(p => p.BELliability).HasPrecision(21, 6);
            builder.Property(p => p.BELrelease).HasPrecision(21, 6);
            builder.Property(p => p.UnwindingBEL).HasPrecision(21, 6);
            builder.Property(p => p.RALiability).HasPrecision(21, 6);
            builder.Property(p => p.RARelease).HasPrecision(21, 6);
            builder.Property(p => p.UnwindingRA).HasPrecision(21, 6);
            builder.Property(p => p.RACurrent).HasPrecision(21, 6);
            builder.Property(p => p.OCIRA).HasPrecision(21, 6);
            builder.Property(p => p.CSMLiability).HasPrecision(21, 6);
            builder.Property(p => p.CSMRelease).HasPrecision(21, 6);
            builder.Property(p => p.UnwindingCSM).HasPrecision(21, 6);
            builder.Property(p => p.CSMRelease2).HasPrecision(21, 6);
            builder.Property(p => p.TotalCSMRelease).HasPrecision(21, 6);
            builder.Property(p => p.LRCliability).HasPrecision(21, 6);
            builder.Property(p => p.LRCRelease).HasPrecision(21, 6);
            builder.Property(p => p.BELreleaseIDR).HasPrecision(21, 6);
            builder.Property(p => p.RAReleaseIDR).HasPrecision(21, 6);
            builder.Property(p => p.CSMReleaseIDR).HasPrecision(21, 6);
            builder.Property(p => p.LRCReleaseIDR).HasPrecision(21, 6);
            builder.Property(p => p.BELreleaseMovement).HasPrecision(21, 6);
            builder.Property(p => p.RAReleaseMovement).HasPrecision(21, 6);
            builder.Property(p => p.CSMReleaseMovement).HasPrecision(21, 6);
            builder.Property(p => p.LRCReleaseMovement).HasPrecision(21, 6);
            builder.Property(p => p.BELreleaseMovementIDR).HasPrecision(21, 6);
            builder.Property(p => p.RAReleaseMovementIDR).HasPrecision(21, 6);
            builder.Property(p => p.LRCReleaseMovementIDR).HasPrecision(21, 6);
            builder.Property(p => p.BELreleaseMovementIDR).HasPrecision(21, 6);
        }
    }
}