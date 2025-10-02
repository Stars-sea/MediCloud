using MediCloud.Domain.Record;
using MediCloud.Domain.Record.ValueObjects;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCloud.Infrastructure.Persistence.Configurations;

public class RecordConfiguration : IEntityTypeConfiguration<Record> {

    private readonly static ValueComparer<IReadOnlyCollection<string>> ImagesValueComparer =
        new(
            (l, r) => l != null && r != null && l.SequenceEqual(r),
            v => v.Aggregate(0, (hash, item) => HashCode.Combine(hash, item.GetHashCode()))
        );

    public void Configure(EntityTypeBuilder<Record> builder) {
        builder.Property(x => x.Id)
               .ValueGeneratedNever()
               .HasConversion(
                   v => v.Value,
                   value => RecordId.Factory.Create(value)
               );

        builder.Property(x => x.OwnerId)
               .ValueGeneratedNever()
               .HasConversion(
                   v => v.Value,
                   value => UserId.Factory.Create(value)
               );

        builder.HasIndex(x => x.OwnerId);

        builder.Property(x => x.Images)
               .HasConversion(
                   x => x.ToArray(),
                   a => a.ToList()
               )
               .Metadata.SetValueComparer(ImagesValueComparer);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }

}
