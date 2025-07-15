using MediCloud.Domain.LiveRoom.ValueObjects;
using MediCloud.Domain.User;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCloud.Infrastructure.Persistence.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User> {

    public void Configure(EntityTypeBuilder<User> builder) { CreateUsersTable(builder); }

    private static void CreateUsersTable(EntityTypeBuilder<User> builder) {
        builder.Property(x => x.Id)
               .ValueGeneratedNever()
               .HasConversion(
                   id => id.Value,
                   value => UserId.Factory.Create(value)
               );

        builder.HasIndex(l => l.LiveRoomId).IsUnique();
        
        builder.Property(x => x.LiveRoomId)
               .HasConversion(
                   id => id!.Value,
                   value => LiveRoomId.Factory.Create(value)
               );
    }

}
