using MassTransit;
using MassTransit.Mediator;
using MediCloud.Application.Record.Contracts;
using MediCloud.Contracts.Record;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Record.ValueObjects;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace MediCloud.Api.Controllers;

[Route("record")]
public class RecordController(
    IMediator mediator
) : ApiController {

    [HttpPost]
    public async Task<ActionResult<CreateRecordResponse>> CreateRecord(IFormFile file) {
        if (file.Length is >= 16 * 1024 * 1024 or 0) {
            return Problem(Errors.Record.RecordInvalidImageSize, Errors.Record.RecordFailedToSaveImage);
        }

        UserId? userId = TryGetUserId();
        if (userId is null) return Problem(Errors.Auth.InvalidCred);

        await using Stream fileStream = file.OpenReadStream();
        var createRecordResult = await mediator.SendRequest(
            new AddRecordCommand(userId, fileStream)
        );

        return createRecordResult.Match(
            result => Ok(new CreateRecordResponse(result.RecordId.ToString(), result.CreatedOn)),
            Problem
        );
    }

    [HttpGet("{recordId}")]
    public async Task<ActionResult<FindRecordResponse>> FindRecord(string recordId) {
        UserId? userId = TryGetUserId();
        if (userId is null) return Problem(Errors.Auth.InvalidCred);

        var findRecordResult = await mediator.SendRequest(
            new FindRecordQuery(RecordId.Factory.Create(Guid.Parse(recordId)))
        );

        return findRecordResult.Match(
            result => Ok(new FindRecordResponse(
                    result.Id.ToString(),
                    result.OwnerId.ToString(),
                    $"/record/image/{result.ImageName}",
                    result.Remarks
                )
            ),
            Problem
        );
    }

    [HttpGet("image/{imageName}")]
    public async Task<ActionResult> GetImage(string imageName) {
        UserId? userId = TryGetUserId();
        if (userId is null) return Problem(Errors.Auth.InvalidCred);

        var findRecordImageResult = await mediator.SendRequest(
            new FindRecordImageQuery(userId, imageName)
        );

        return findRecordImageResult.Match<ActionResult>(
            result => File(result, "image/jpeg"),
            Problem
        );
    }

}
