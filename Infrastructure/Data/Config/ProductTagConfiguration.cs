using eShop_Mvc.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop_Mvc.Infrastructure.Data.Config
{
    public class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
    {
        public void Configure(EntityTypeBuilder<ProductTag> builder)
        {
            builder.HasKey(pt => new { pt.ProductId, pt.TagId });
            builder.Property(pt => pt.TagId).HasMaxLength(50).IsRequired();
            builder.HasOne(pt => pt.Tag).WithMany(t => t.ProductTags).HasForeignKey(pt => pt.TagId).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(pt => pt.Product).WithMany(p => p.ProductTags).HasForeignKey(pt => pt.ProductId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}