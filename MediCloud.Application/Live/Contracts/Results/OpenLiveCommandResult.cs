namespace MediCloud.Application.Live.Contracts.Results;

public sealed record OpenLiveCommandResult(
    string LiveId,
    string LiveName,
    string LiveWatchUrl,
    string LivePostUrl,
    string Passphrase
);
