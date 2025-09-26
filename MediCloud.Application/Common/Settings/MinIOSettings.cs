namespace MediCloud.Application.Common.Settings;

public class MinIOSettings {

    public const string SectionKey = "MinIO";

    public string AccessKey { get; set; } = null!;

    public string SecretKey { get; set; } = null!;

}
