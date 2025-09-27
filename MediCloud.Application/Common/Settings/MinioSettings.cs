namespace MediCloud.Application.Common.Settings;

public class MinioSettings {

    public const string SectionKey = "MinIO";

    public string Endpoint { get; set; } = null!;

    public string AccessKey { get; set; } = null!;

    public string SecretKey { get; set; } = null!;

}
