using System.Text.Json.Serialization;
using MediCloud.Domain.Common.Models;
using MediCloud.Domain.Record.ValueObjects;
using MediCloud.Domain.User.ValueObjects;

#pragma warning disable CS8618

namespace MediCloud.Domain.Record;

public class Record : AggregateRoot<RecordId, Guid> {

    [JsonConstructor]
    private Record() { }

    private Record(
        RecordId recordId,
        UserId   ownerId,
        DateTime createdOn,
        string   imageName,
        string   remarks
    ) : base(recordId) {
        OwnerId   = ownerId;
        CreatedOn = createdOn;
        ImageName = imageName;
        Remarks   = remarks;
    }

    public UserId OwnerId { get; private set; }

    public DateTime CreatedOn { get; private set; }

    public string ImageName { get; private set; }

    public string Remarks { get; private set; }

    public bool IsDeleted { get; private set; } = false;

    public void Delete() {
        IsDeleted = true;
    }

    public static class Factory {

        public static Record Create(UserId ownerId, string imageName, string remarks) {
            return new Record(RecordId.Factory.CreateUnique(), ownerId, DateTime.UtcNow, imageName, remarks);
        }

    }

}
