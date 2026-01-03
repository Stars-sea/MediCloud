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

    [HttpPut]
    public async Task<ActionResult<CreateLiveResponse>> CreateLive([FromBody] CreateLiveRequest request) {
        UserId? id = TryGetUserId();
        if (id == null)
            return Problem(Errors.Auth.InvalidCred);

        var createResult = await mediator.SendRequest(request.MapCommand(id));
        return createResult.Match(r => Ok(new CreateLiveResponse(r.ToString())), Problem);
    }

    [HttpGet("{liveId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LiveStatusResponse))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetailedLiveStatusResponse))]
    public async Task<ActionResult> GetLiveStatus(Guid liveId) {
        UserId? id           = TryGetUserId();
        var     statusResult = await mediator.SendRequest(liveId.ToLiveStatusQuery());
        return statusResult.Match(
            r => Ok(r.OwnerId == id ? r.MapDetailedResp() : r.MapResp()),
            Problem
        );
    }

    [HttpPatch("{liveId:guid}")]
    public async Task<ActionResult> UpdateLiveStatus(Guid liveId, [FromBody] UpdateLiveStatusRequest request) {
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
