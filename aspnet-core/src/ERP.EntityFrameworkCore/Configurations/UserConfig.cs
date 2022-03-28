
using ERP.Authorization.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Configurations
{
    public class UserConfig
    {
        public static void Configure(EntityTypeBuilder<User> entityBuilder)
        {
            entityBuilder.Property(e => e.Address).HasMaxLength(1024).IsUnicode(false);
        }
    }
}