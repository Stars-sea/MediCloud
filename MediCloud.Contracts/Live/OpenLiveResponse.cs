namespace MediCloud.Contracts.Live;

public record OpenLiveResponse(
    string LiveId,
    string LiveName,
    string LiveWatchUrl,
    string LivePostUrl,
    string Passphrase
);
