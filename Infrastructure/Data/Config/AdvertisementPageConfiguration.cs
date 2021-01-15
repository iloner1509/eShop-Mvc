using eShop_Mvc.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop_Mvc.Infrastructure.Data.Config
{
    public class AdvertisementPageConfiguration : IEntityTypeConfiguration<AdvertisementPage>
    {
        public void Configure(EntityTypeBuilder<AdvertisementPage> builder)
        {
            builder.Property(ap => ap.Id).HasMaxLength(20).HasColumnType("varchar(20)").IsRequired();
        }
    }
}