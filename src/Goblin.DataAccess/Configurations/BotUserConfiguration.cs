using Goblin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goblin.DataAccess.Configurations
{
    public class BotUserConfiguration : IEntityTypeConfiguration<BotUser>
    {
        public void Configure(EntityTypeBuilder<BotUser> builder)
        {
            builder.HasKey(x => x.VkId);
            builder.Property(x => x.VkId).ValueGeneratedNever();

            builder.Property(x => x.WeatherCity).HasMaxLength(100).HasDefaultValue(string.Empty);
            builder.Property(x => x.NarfuGroup).HasDefaultValue(0);
            builder.Property(x => x.IsAdmin).HasDefaultValue(false);
            builder.Property(x => x.IsErrorsEnabled).HasDefaultValue(true);
        }
    }
}