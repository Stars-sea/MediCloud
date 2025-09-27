using MediCloud.Domain.Record;
using MediCloud.Domain.Record.ValueObjects;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCloud.Infrastructure.Persistence.Configurations;

public class RecordConfiguration : IEntityTypeConfiguration<Record> {

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

        builder.HasQueryFilter(x => !x.IsDeleted);
    }

}
