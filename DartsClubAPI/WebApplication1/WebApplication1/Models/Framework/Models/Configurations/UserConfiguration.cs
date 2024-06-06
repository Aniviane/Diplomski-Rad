using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication1.Models.Framework.Models.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.ID);
            builder.HasMany(x => x.Reservations).WithOne(r => r.User).HasForeignKey(r => r.UserId);
            builder.HasOne(x => x.Picture);
        }
    }
}
