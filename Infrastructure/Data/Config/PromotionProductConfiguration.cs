using eShop_Mvc.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop_Mvc.Infrastructure.Data.Config
{
    public class PromotionProductConfiguration : IEntityTypeConfiguration<PromotionProduct>
    {
        public void Configure(EntityTypeBuilder<PromotionProduct> builder)
        {
            builder.HasKey(pp => new { pp.ProductId, pp.PromotionId });
            builder.HasOne(pp => pp.Product).WithMany(p => p.PromotionProducts).HasForeignKey(pp => pp.ProductId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pp => pp.Promotion).WithMany(p => p.PromotionProducts).HasForeignKey(pp => pp.PromotionId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}