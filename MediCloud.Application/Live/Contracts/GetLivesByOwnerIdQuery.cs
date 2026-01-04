using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Live.Contracts;

public sealed record GetLivesByOwnerIdQuery(
    UserId OwnerId
);
