using eShop_Mvc.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop_Mvc.Infrastructure.Data.Config
{
    public class AnnouncementUserConfiguration : IEntityTypeConfiguration<AnnouncementUser>
    {
        public void Configure(EntityTypeBuilder<AnnouncementUser> builder)
        {
            builder.HasOne(au => au.Announcement).WithMany(au => au.AnnouncementUsers)
                .HasForeignKey(au => au.AnnouncementId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}