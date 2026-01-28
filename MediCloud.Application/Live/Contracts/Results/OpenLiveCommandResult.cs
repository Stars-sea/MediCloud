using MediCloud.Domain.Live.ValueObjects;

namespace MediCloud.Application.Live.Contracts.Results;

public sealed record OpenLiveCommandResult(
    LiveId LiveId,
    string LiveName,
    Uri    PostUrl,
    string Passphrase
);
