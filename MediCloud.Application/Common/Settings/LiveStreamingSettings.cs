namespace MediCloud.Application.Common.Settings;

public class LiveStreamingSettings {

    public const string SectionKey = "LiveStreaming";
    
    public string SrtServer { get; set; } = null!;
    
    public string StoragePath { get; set; } = null!;

    public int Timeout { get; set; }

    public int Latency { get; set; }

    public int Ffs { get; set; }

}
