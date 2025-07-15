using MediCloud.Domain.Live;
using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.LiveRoom.ValueObjects;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCloud.Infrastructure.Persistence.Configurations;

public class LiveConfiguration : IEntityTypeConfiguration<Live> {

    public void Configure(EntityTypeBuilder<Live> builder) {
        builder.Property(x => x.Id)
               .ValueGeneratedNever()
               .HasConversion(
                   id => id.Value,
                   value => LiveId.Factory.Create(value)
               );

        builder.HasIndex(x => x.LiveRoomId).IsUnique();

        builder.Property(x => x.LiveRoomId)
               .IsRequired()
               .HasConversion(
                   id => id.Value,
                   value => LiveRoomId.Factory.Create(value)
               );

        builder.HasIndex(x => x.OwnerId).IsUnique();

        builder.Property(x => x.OwnerId)
               .IsRequired()
               .HasConversion(
                   id => id.Value,
                   value => UserId.Factory.Create(value)
               );
    }

}
