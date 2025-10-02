using MassTransit.Mediator;
using MediCloud.Application.Common.Contracts;
using MediCloud.Domain.Record.ValueObjects;

namespace MediCloud.Application.Record.Contracts;

public record GetRecordImageUrlQuery(
    RecordId Id,
    string   ImageName
) : Request<Result<string>>;
