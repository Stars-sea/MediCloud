using MassTransit;
using MassTransit.Mediator;
using MediCloud.Application.Live.Contracts;
using MediCloud.Contracts.Live;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using LiveStatus=MediCloud.Domain.Live.LiveStatus;

namespace MediCloud.Api.Controllers;

[Route("lives")]
public class LiveController(
    IMediator mediator
) : ApiController {

    [HttpPost("/live-rooms")]
    public async Task<ActionResult> CreateLiveRoom([FromBody] CreateLiveRoomRequest request) {
        UserId? id = TryGetUserId();
        if (id == null)
            return Problem(Errors.User.UserNotFound);

        var createResult = await mediator.SendRequest(
            new CreateLiveRoomCommand(id, request.RoomName)
        );

        return createResult.Match<ActionResult>(Ok, Problem);
    }

    [HttpPut]
    public async Task<ActionResult> CreateLive([FromBody] CreateLiveRequest request) {
        UserId? id = TryGetUserId();
        if (id == null)
            return Problem(Errors.User.UserNotFound);

        var createResult = await mediator.SendRequest(
            new CreateLiveCommand(id, request.LiveName)
        );

        return createResult.Match<ActionResult>(r => Ok(new CreateLiveResponse(r.ToString())), Problem);
    }

    [HttpGet("{liveId}")]
    public async Task<ActionResult<LiveStatusResponse>> GetLiveStatus(string liveId) {
        var statusResult = await mediator.SendRequest(
            new GetLiveStatusQuery(LiveId.Factory.Create(Guid.Parse(liveId)))
        );

        return statusResult.Match<ActionResult>(
            r => Ok(new LiveStatusResponse(
                r.LiveId,
                r.OwnerId,
                r.Status switch {
                    LiveStatus.Pending   => Contracts.Live.LiveStatus.Pending,
                    LiveStatus.Streaming => Contracts.Live.LiveStatus.Streaming,
                    _                    => Contracts.Live.LiveStatus.Stopped
                })
            ),
            Problem
        );
    }

    [HttpPatch("{liveId}")]
    public async Task<ActionResult<UpdateLiveStatusResponse>> UpdateLiveStatus(string liveId, [FromBody] UpdateLiveStatusRequest request) {
        UserId? id = TryGetUserId();
        if (id == null)
            return Problem(Errors.User.UserNotFound);

        (string liveName, Contracts.Live.LiveStatus status) = request;
        var updateResult = await mediator.SendRequest(
            new UpdateLiveStatusCommand(
                id,
                LiveId.Factory.Create(Guid.Parse(liveId)),
                liveName,
                status switch {
                    Contracts.Live.LiveStatus.Pending   => LiveStatus.Pending,
                    Contracts.Live.LiveStatus.Streaming => LiveStatus.Streaming,
                    _                                   => LiveStatus.Stopped
                }
            )
        );
        return updateResult.Match<ActionResult>(
            () => Ok(new UpdateLiveStatusResponse(
                liveId,
                id.ToString(),
                status
            )),
            Problem
        );
    }

}
