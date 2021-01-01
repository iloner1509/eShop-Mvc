using eShop_Mvc.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace eShop_Mvc.Infrastructure.Data.Config
{
    public class BillDetailConfiguration : IEntityTypeConfiguration<BillDetail>
    {
        public void Configure(EntityTypeBuilder<BillDetail> builder)
        {
            builder.HasOne(bd => bd.Bill).WithMany(b => b.BillDetails).HasForeignKey(bd => bd.BillId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(bd => bd.Product).WithMany(p => p.BillDetails).HasForeignKey(bd => bd.ProductId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}