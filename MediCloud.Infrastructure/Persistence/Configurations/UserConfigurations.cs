using MediCloud.Domain.User;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCloud.Infrastructure.Persistence.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User> {

    public void Configure(EntityTypeBuilder<User> builder) { CreateUsersTable(builder); }

    private static void CreateUsersTable(EntityTypeBuilder<User> builder) {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedNever()
               .HasConversion(
                   id => id.ToString(),
                   id => UserId.Create(id)
               );
    }

}
