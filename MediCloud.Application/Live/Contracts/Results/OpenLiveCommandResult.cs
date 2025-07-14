namespace MediCloud.Application.Live.Contracts.Results;

public record OpenLiveCommandResult(
    string LiveId,
    string LiveName,
    string LiveWatchUrl,
    string LivePostUrl,
    string Passphrase
);
