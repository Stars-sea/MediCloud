namespace MediCloud.Application.Common.Settings;

public class LiveStreamingSettings {

    public const string SectionKey = "LiveStreaming";
    
    public string SrtServer { get; set; } = null!;
    
    public string StoragePath { get; set; } = null!;

    public ulong Timeout { get; set; }

    public ulong Latency { get; set; }

    public ulong Ffs { get; set; }

}
