using MediCloud.Application.Record.Contracts;
using MediCloud.Application.Record.Contracts.Result;
using MediCloud.Contracts.Record;
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

    public static partial RecordResponse MapResp(this FindRecordByIdQueryResult record);

    [MapperIgnoreSource(nameof(FindRecordByIdQueryResult.ImageUrls))]
    [MapperIgnoreSource(nameof(FindRecordByIdQueryResult.Remarks))]
    private static partial SimpleRecordResponse MapSimpleResp(this FindRecordByIdQueryResult record);

    public static partial ICollection<SimpleRecordResponse> MapResps(this List<FindRecordByIdQueryResult> records);

    public static partial CreateRecordResponse MapResp(this AddRecordCommandResult result);

}
