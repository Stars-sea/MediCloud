using MassTransit;
using MassTransit.Mediator;
using MediCloud.Application.Live.Contracts;
using MediCloud.Contracts.Live;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace MediCloud.Api.Controllers;

[Route("live")]
public class LiveController(
    IMediator mediator
) : ApiController {

    [HttpPost("room")]
    public async Task<ActionResult> Handle([FromBody] CreateLiveRoomRequest request) {
        UserId? id = TryGetUserId();
        if (id == null)
            return Problem(Errors.User.UserNotFound);

        var createResult = await mediator.SendRequest(
            new CreateLiveRoomCommand(id, request.RoomName)
        );

        return createResult.Match<ActionResult>(Ok, Problem);
    }

    [HttpPost("open")]
    public async Task<ActionResult<OpenLiveResponse>> OpenLive([FromBody] OpenLiveRequest request) {
        UserId? id = TryGetUserId();
        if (id == null)
            return Problem(Errors.User.UserNotFound);

        var openResult = await mediator.SendRequest(
            new OpenLiveCommand(request.Name, id)
        );

        return openResult.Match(
            r => Ok(new OpenLiveResponse(r.LiveId, r.LiveName, r.LiveWatchUrl, r.LivePostUrl, r.Passphrase)),
            Problem
        );
    }

    [HttpGet("{liveId}/status")]
    public async Task<ActionResult<LiveStatusResponse>> GetLiveStatus(string liveId) {
        var statusResult = await mediator.SendRequest(new GetLiveStatusQuery(liveId));

        return statusResult.Match(
            result => Ok(new LiveStatusResponse(result.LiveId, result.OwnerId, result.LiveStatus)),
            Problem
        );
    }

}
