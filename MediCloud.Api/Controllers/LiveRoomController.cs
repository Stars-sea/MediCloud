using MassTransit;
using MassTransit.Mediator;
using MediCloud.Api.Common.Mappers;
using MediCloud.Application.LiveRoom.Contracts.Results;
using MediCloud.Contracts.LiveRoom;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace MediCloud.Api.Controllers;

[Route("live-rooms")]
public class LiveRoomController(
    IMediator mediator
) : ApiController {

    [HttpPut]
    public async Task<ActionResult> CreateLiveRoom([FromBody] CreateLiveRoomRequest request) {
        UserId? id = TryGetUserId();
        if (id == null)
            return Problem(Errors.Auth.InvalidCred);

        var createResult = await mediator.SendRequest(request.MapCommand(id));
        return createResult.Match<ActionResult>(Ok, Problem);
    }

    [HttpGet("{liveRoomId:guid}")]
    public async Task<ActionResult<GetLiveRoomInfoResponse>> GetLiveRoomInfo([FromRoute] Guid liveRoomId) {
        Result<GetLiveRoomInfoQueryResult> getResult = await mediator.SendRequest(liveRoomId.ToLiveRoomId().ToGetLiveRoomQuery());
        return getResult.Match(r => Ok(r.MapResp()), Problem);
    }

    [HttpGet("me")]
    public async Task<ActionResult<GetLiveRoomInfoResponse>> GetLiveRoomInfoByOwner() {
        UserId? ownerId = TryGetUserId();
        if (ownerId == null)
            return Problem(Errors.Auth.InvalidCred);

        Result<GetLiveRoomInfoQueryResult> getResult = await mediator.SendRequest(ownerId.ToGetLiveRoomQuery());
        return getResult.Match(r => Ok(r.MapResp()), Problem);
    }

}
