namespace MediCloud.Contracts.Live;

public record LiveStatusResponse(
    string LiveId,
    string OwnerId,
    string Status
);
