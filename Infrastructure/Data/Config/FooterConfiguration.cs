using eShop_Mvc.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop_Mvc.Infrastructure.Data.Config
{
    public class FooterConfiguration : IEntityTypeConfiguration<Footer>
    {
        public void Configure(EntityTypeBuilder<Footer> builder)
        {
            builder.Property(f => f.Id).HasMaxLength(50).HasColumnType("varchar(50)").IsRequired();
        }
    }
}