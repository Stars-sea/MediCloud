using MassTransit;
using MassTransit.Mediator;
using MediCloud.Application.Record.Contracts;
using MediCloud.Contracts.Record;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Record;
using MediCloud.Domain.Record.ValueObjects;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace MediCloud.Api.Controllers;

[Route("record")]
public class RecordController(
    IMediator mediator
) : ApiController {

    [NonAction]
    private static FindRecordResponse MapRecord(Record record) {
        return new FindRecordResponse(
            record.Id.ToString(),
            record.OwnerId.ToString(),
            $"/record/image/{record.ImageName}",
            record.Remarks,
            record.CreatedOn
        );
    }

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
            result => Ok(MapRecord(result)),
            Problem
        );
    }

    [HttpGet]
    public async Task<ActionResult<List<FindRecordResponse>>> FindRecords() {
        UserId? userId = TryGetUserId();
        if (userId is null) return Problem(Errors.Auth.InvalidCred);

        var findRecordsResult = await mediator.SendRequest(
            new FindRecordsByOwnerQuery(userId)
        );
        return findRecordsResult.Match(
            results => Ok(results.Select(MapRecord).ToList()),
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
            result => File(result, "image/jpeg", $"{imageName}.jpg"),
            Problem
        );
    }

}
