using MassTransit;
using MediCloud.Application.Live.Contracts;
using Microsoft.Extensions.Logging;

namespace MediCloud.Application.Live.Consumers;

public class StreamRetrievedResponseConsumer(
    ILogger<StreamRetrievedResponseConsumer> logger
) : IConsumer<StreamRetrievedResponse> {

    public Task Consume(ConsumeContext<StreamRetrievedResponse> context) {
        StreamRetrievedResponse response = context.Message;

        logger.LogInformation("Consumed stream retrieved response {id} with code {code} ({url} -> {path})",
            response.LiveId, response.Code, response.Url, response.Path
        );
        
        

        return Task.CompletedTask;
    }

}
