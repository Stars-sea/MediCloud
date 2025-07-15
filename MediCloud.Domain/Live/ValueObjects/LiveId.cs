using MediCloud.Domain.Common.Models;

namespace MediCloud.Domain.Live.ValueObjects;

public sealed class LiveId : AggregateRootId<Guid> {

    public override Guid Value { get; }
    
    private LiveId(Guid value) { Value = value; }

    public static class Factory {

        public static LiveId CreateUnique() { return new LiveId(Guid.NewGuid()); }

        public static LiveId Create(Guid value) { return new LiveId(value); }

    }

}
