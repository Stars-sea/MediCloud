using MediCloud.Application.Live.Contracts;
using MediCloud.Application.Live.Contracts.Results;
using MediCloud.Contracts.Live;
using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.User.ValueObjects;
using Riok.Mapperly.Abstractions;
using ContractStatus=MediCloud.Contracts.Live.LiveStatus;
using DomainStatus=MediCloud.Domain.Live.Enums.LiveStatus;

namespace MediCloud.Api.Common.Mappers;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public static partial class LiveMappers {

    public static LiveId ToLiveId(this Guid liveId) => LiveId.Factory.Create(liveId);

    public static partial CreateLiveCommand MapCommand(this CreateLiveRequest request, UserId userId);

    public static GetLiveByIdQuery ToLiveStatusQuery(this Guid liveId) => new(liveId.ToLiveId());

    public static partial UpdateLiveStatusCommand MapCommand(this UpdateLiveStatusRequest request, LiveId liveId, UserId userId);

    public static partial DomainStatus MapDomain(this ContractStatus result);

    [MapperIgnoreSource(nameof(GetLiveByIdQueryResult.PostUrl))]
    [MapperIgnoreSource(nameof(GetLiveByIdQueryResult.Passphrase))]
    public static partial LiveInfoResponse MapResp(this GetLiveByIdQueryResult result);

    public static partial DetailedLiveInfoResponse MapDetailedResp(this GetLiveByIdQueryResult result);

}
