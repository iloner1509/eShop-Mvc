using eShop_Mvc.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop_Mvc.Infrastructure.Data.Config
{
    public class BlogTagConfiguration : IEntityTypeConfiguration<BlogTag>
    {
        public void Configure(EntityTypeBuilder<BlogTag> builder)
        {
            builder.HasKey(bt => new { bt.BlogId, bt.TagId });
            builder.Property(bt => bt.TagId).HasMaxLength(50).HasColumnType("varchar(50)").IsRequired();
            builder.Property(bt => bt.Id).ValueGeneratedOnAdd();
            builder.HasOne(bt => bt.Tag).WithMany(t => t.BlogTags).HasForeignKey(bt => bt.TagId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(bt => bt.Blog).WithMany(b => b.BlogTags).HasForeignKey(bt => bt.BlogId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}