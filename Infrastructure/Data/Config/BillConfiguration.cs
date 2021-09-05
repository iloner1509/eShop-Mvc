using eShop_Mvc.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop_Mvc.Infrastructure.Data.Config
{
    public class BillConfiguration : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            //builder.Property(b => b.Id).UseIdentityColumn();
            builder.HasOne(b => b.User).WithMany(au => au.Bills).HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}