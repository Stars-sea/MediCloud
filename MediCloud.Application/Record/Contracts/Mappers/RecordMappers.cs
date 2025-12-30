using MediCloud.Application.Record.Contracts.Result;
using Riok.Mapperly.Abstractions;

namespace MediCloud.Application.Record.Contracts.Mappers;

[Mapper]
internal static partial class RecordMappers {

    [MapperIgnoreSource(nameof(Domain.Record.Record.Title))]
    [MapperIgnoreSource(nameof(Domain.Record.Record.Remarks))]
    [MapperIgnoreSource(nameof(Domain.Record.Record.OwnerId))]
    [MapperIgnoreSource(nameof(Domain.Record.Record.Images))]
    [MapperIgnoreSource(nameof(Domain.Record.Record.IsDeleted))]
    [MapProperty(nameof(Domain.Record.Record.Id), nameof(AddRecordCommandResult.RecordId))]
    public static partial AddRecordCommandResult MapAddRecordResult(this Domain.Record.Record record);

    [MapperIgnoreSource(nameof(Domain.Record.Record.IsDeleted))]
    [MapPropertyFromSource(nameof(FindRecordByIdQueryResult.ImageUrls), Use = nameof(MapImages))]
    public static partial FindRecordByIdQueryResult MapFindRecordByIdResult(this Domain.Record.Record record);

    private static IEnumerable<string> MapImages(Domain.Record.Record record)
        => record.Images.Select(s => $"/record/{record.Id}/image/{s}");

    public static partial List<FindRecordByIdQueryResult> MapFindRecordsByOwnerIdResult(this List<Domain.Record.Record> records);

}
