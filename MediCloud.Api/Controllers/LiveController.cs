using MassTransit;
using MassTransit.Mediator;
using MediCloud.Api.Common.Mappers;
using MediCloud.Contracts.Live;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace MediCloud.Api.Controllers;

[Route("lives")]
public class LiveController(
    IMediator mediator
) : ApiController {

    [HttpPost("/live-rooms")]
    public async Task<ActionResult> CreateLiveRoom([FromBody] CreateLiveRoomRequest request) {
        UserId? id = TryGetUserId();
        if (id == null)
            return Problem(Errors.Auth.InvalidCred);

        var createResult = await mediator.SendRequest(request.MapCommand(id));
        return createResult.Match<ActionResult>(Ok, Problem);
    }

    [HttpPut]
    public async Task<ActionResult<CreateLiveResponse>> CreateLive([FromBody] CreateLiveRequest request) {
        UserId? id = TryGetUserId();
        if (id == null)
            return Problem(Errors.Auth.InvalidCred);

        var createResult = await mediator.SendRequest(request.MapCommand(id));
        return createResult.Match(r => Ok(new CreateLiveResponse(r.ToString())), Problem);
    }

    [HttpGet("{liveId}")]
    public async Task<ActionResult<LiveStatusResponse>> GetLiveStatus(string liveId) {
        var statusResult = await mediator.SendRequest(liveId.ToLiveStatusQuery());
        return statusResult.Match(r => Ok(r.MapResp()), Problem);
    }

    [HttpPatch("{liveId}")]
    public async Task<ActionResult> UpdateLiveStatus(string liveId, [FromBody] UpdateLiveStatusRequest request) {
        UserId? id = TryGetUserId();
        if (id == null)
            return Problem(Errors.Auth.InvalidCred);

        var updateResult = await mediator.SendRequest(request.MapCommand(liveId.ToLiveId(), id));

        if (!updateResult.IsSuccess)
            return Problem(updateResult.Errors);

        return RedirectToAction(nameof(GetLiveStatus),
            new {
                liveId
            }
        );
    }

}
