using eShop_Mvc.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop_Mvc.Infrastructure.Data.Config
{
    public class AdvertisementConfiguration : IEntityTypeConfiguration<Advertisement>
    {
        public void Configure(EntityTypeBuilder<Advertisement> builder)
        {
            //builder.Property(ad => ad.Id).HasMaxLength(50).HasColumnType("varchar(20)").IsRequired();
            builder.HasOne(a => a.AdvertisementPosition).WithMany(ap => ap.Advertisements).HasForeignKey(a => a.PositionId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}