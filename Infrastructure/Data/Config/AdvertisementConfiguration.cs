using eShop_Mvc.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop_Mvc.Infrastructure.Data.Config
{
    public class AdvertisementConfiguration : IEntityTypeConfiguration<Advertisement>
    {
        public void Configure(EntityTypeBuilder<Advertisement> builder)
        {
            builder.HasOne(a => a.AdvertisementPosition).WithMany(ap => ap.Advertisements)
                .HasForeignKey(a => a.PositionId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}