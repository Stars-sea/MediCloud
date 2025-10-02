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
        RecordId id,
        UserId   ownerId,
        string   title,
        string   remarks,
        DateTime createdOn
    ) : base(id) {
        OwnerId   = ownerId;
        Title     = title;
        Remarks   = remarks;
        CreatedOn = createdOn;
    }

    public UserId OwnerId { get; private set; }

    public string Title { get; private set; }

    private readonly List<string> _images = [];

    public IReadOnlyList<string> Images => _images;

    public string Remarks { get; private set; }

    public DateTime CreatedOn { get; private set; }

    public bool IsDeleted { get; private set; }

    public void AddImage(string image) {
        if (_images.Contains(image) || _images.Count > 10) return;
        _images.Add(image);
    }

    public void Delete() { IsDeleted = true; }

    public static class Factory {

        public static Record Create(UserId ownerId, string title, string remarks) {
            return new Record(RecordId.Factory.CreateUnique(), ownerId, title, remarks, DateTime.UtcNow);
        }

    }

}
