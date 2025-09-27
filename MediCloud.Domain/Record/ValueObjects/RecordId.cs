using System.Text.Json.Serialization;
using MediCloud.Domain.Common.Models;

namespace MediCloud.Domain.Record.ValueObjects;

public class RecordId : AggregateRootId<Guid> {

    public override Guid Value { get; }

    [JsonConstructor]
    private RecordId(Guid value) { Value = value; }

    public static implicit operator Guid(RecordId userId) => userId.Value;

    public static class Factory {

        public static RecordId CreateUnique() { return new RecordId(Guid.NewGuid()); }

        public static RecordId Create(Guid value) { return new RecordId(value); }

    }
    
}
