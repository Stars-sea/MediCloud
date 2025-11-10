namespace MediCloud.Application.Common.Settings;

public class LivestreamSettings {

    public const string SectionKey = "Livestream";

    public string GrpcServer { get; set; } = null!;

    public string SrtServer { get; set; } = null!;

}
