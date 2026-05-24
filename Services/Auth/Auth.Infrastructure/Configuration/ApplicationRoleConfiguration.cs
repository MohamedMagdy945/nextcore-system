using Auth.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Configuration
{
    public class ApplicationRoleConfiguration
        : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {

            builder.HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}
