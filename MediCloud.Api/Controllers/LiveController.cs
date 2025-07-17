using System.IdentityModel.Tokens.Jwt;
using MassTransit;
using MassTransit.Mediator;
using MediCloud.Application.Live.Contracts;
using MediCloud.Contracts.Live;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace MediCloud.Api.Controllers;

[Route("live")]
public class LiveController(
    IMediator mediator
) : ApiController {

    [HttpPost("open")]
    public async Task<ActionResult<OpenLiveResponse>> OpenLive([FromBody] OpenLiveRequest request) {
        UserId id;
        try {
            string userId = User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value;
            id = UserId.Factory.Create(Guid.Parse(userId));
        }
        catch (Exception e) { return Problem(e.Message); }

        var openResult = await mediator.SendRequest(new OpenLiveCommand(
                request.Name, id
            )
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
