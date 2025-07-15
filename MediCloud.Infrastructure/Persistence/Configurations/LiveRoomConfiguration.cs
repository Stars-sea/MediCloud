using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.LiveRoom;
using MediCloud.Domain.LiveRoom.Enums;
using MediCloud.Domain.LiveRoom.ValueObjects;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCloud.Infrastructure.Persistence.Configurations;

public sealed class LiveRoomConfiguration : IEntityTypeConfiguration<LiveRoom> {

    public void Configure(EntityTypeBuilder<LiveRoom> builder) {
        ConfigureLiveRoomsTable(builder);
        ConfigureLiveIdsTable(builder);
    }

    private static void ConfigureLiveRoomsTable(EntityTypeBuilder<LiveRoom> builder) {
        builder.Property(l => l.Id)
               .ValueGeneratedNever()
               .HasConversion(
                   id => id.Value,
                   value => LiveRoomId.Factory.Create(value)
               );

        builder.Property(l => l.OwnerId)
               .IsRequired()
               .HasConversion(
                   id => id.Value,
                   value => UserId.Factory.Create(value)
               );
        
        builder.HasIndex(l => l.OwnerId).IsUnique();

        builder.Property(l => l.Status).HasConversion<string>();

        builder.HasQueryFilter(l => l.Status != LiveRoomStatus.Deleted);
    }

    private static void ConfigureLiveIdsTable(EntityTypeBuilder<LiveRoom> builder) {
        builder.OwnsMany(l => l.LiveIds, lid => {
                lid.ToTable("LiveLiveRoomIds");

                lid.WithOwner().HasForeignKey(nameof(LiveRoomId));

                lid.HasKey("Id");

                lid.Property(l => l.Value)
                   .ValueGeneratedNever()
                   .HasColumnName("LiveLiveRoomId");
            }
        );

        builder.Metadata.FindNavigation(nameof(LiveRoom.LiveIds))
               ?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }

}
