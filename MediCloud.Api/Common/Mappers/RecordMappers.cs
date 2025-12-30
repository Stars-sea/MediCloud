using MediCloud.Application.Record.Contracts;
using MediCloud.Application.Record.Contracts.Result;
using MediCloud.Contracts.Record;
using MediCloud.Domain.Record;
using MediCloud.Domain.Record.ValueObjects;
using MediCloud.Domain.User.ValueObjects;
using Riok.Mapperly.Abstractions;

namespace MediCloud.Api.Common.Mappers;

[Mapper]
public static partial class RecordMappers {

    public static RecordId ToRecordId(this Guid recordId)
        => RecordId.Factory.Create(recordId);

    public static partial AddRecordCommand MapCommand(this CreateRecordRequest request, UserId userId);

    public static FindRecordByIdQuery ToFindByIdQuery(this Guid recordId)
        => new(recordId.ToRecordId());

    [MapperIgnoreSource(nameof(Record.IsDeleted))]
    [MapPropertyFromSource(nameof(Record.Images), Use = nameof(MapImages))]
    public static partial RecordResponse MapResp(this Record record);

    private static IEnumerable<string> MapImages(Record record)
        => record.Images.Select(s => $"/record/{record.Id}/image/{s}");

    [MapperIgnoreSource(nameof(Record.IsDeleted))]
    [MapperIgnoreSource(nameof(Record.Images))]
    [MapperIgnoreSource(nameof(Record.Remarks))]
    private static partial SimpleRecordResponse MapSimpleResp(this Record record);

    public static partial ICollection<SimpleRecordResponse> MapResps(this ICollection<Record> records);


    public static partial CreateRecordResponse MapResp(this AddRecordCommandResult result);

}
