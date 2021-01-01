using eShop_Mvc.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop_Mvc.Infrastructure.Data.Config
{
    public class AdvertisementPositionConfiguration : IEntityTypeConfiguration<AdvertisementPosition>
    {
        public void Configure(EntityTypeBuilder<AdvertisementPosition> builder)
        {
            builder.Property(ap => ap.Id).HasMaxLength(50).HasColumnType("varchar(50)").IsRequired();
            builder.HasOne(ap => ap.AdvertisementPage).WithMany(apg => apg.AdvertisementPositions)
                .HasForeignKey(ap => ap.PageId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}