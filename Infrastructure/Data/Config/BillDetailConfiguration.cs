using eShop_Mvc.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop_Mvc.Infrastructure.Data.Config
{
    public class BillDetailConfiguration : IEntityTypeConfiguration<BillDetail>
    {
        public void Configure(EntityTypeBuilder<BillDetail> builder)
        {
            builder.HasOne(bd => bd.Bill).WithMany(b => b.BillDetails).HasForeignKey(bd => bd.BillId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(bd => bd.Product).WithMany(p => p.BillDetails).HasForeignKey(bd => bd.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}