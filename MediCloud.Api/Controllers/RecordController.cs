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
    private static RecordResponse MapRecord(Record record) {
        string id = record.Id.ToString();
        return new RecordResponse(
            id,
            record.OwnerId.ToString(),
            record.Title,
            record.Images.Select(s => $"/record/{id}/image/{s}"),
            record.Remarks,
            record.CreatedOn
        );
    }

    [HttpPost]
    public async Task<ActionResult<CreateRecordResponse>> CreateRecord(CreateRecordRequest request) {
        UserId? userId = TryGetUserId();
        if (userId is null) return Problem(Errors.Auth.InvalidCred);

        var createRecordResult = await mediator.SendRequest(
            new AddRecordCommand(userId, request.Title, request.Remarks)
        );

        return createRecordResult.Match(
            result => Ok(new CreateRecordResponse(result.RecordId.ToString(), result.CreatedOn)),
            Problem
        );
    }

    [HttpGet("{recordId}")]
    public async Task<ActionResult<RecordResponse>> FindRecord(string recordId) {
        UserId? userId = TryGetUserId();
        if (userId is null) return Problem(Errors.Auth.InvalidCred);

        var findRecordResult = await mediator.SendRequest(
            new FindRecordByIdQuery(RecordId.Factory.Create(Guid.Parse(recordId)))
        );

        return findRecordResult.Match(
            result => Ok(MapRecord(result)),
            Problem
        );
    }

    [HttpPost("{recordId}/image")]
    public async Task<ActionResult<string>> UploadImage(string recordId, IFormFile file) {
        UserId? userId = TryGetUserId();
        if (userId is null) return Problem(Errors.Auth.InvalidCred);

        RecordId id = RecordId.Factory.Create(Guid.Parse(recordId));

        var verifyOwnerResult = await mediator.SendRequest(
            new VerifyRecordOwnerQuery(id, userId)
        );
        if (!verifyOwnerResult.IsSuccess)
            return Problem(verifyOwnerResult.Errors);

        await using var stream = file.OpenReadStream();
        var addImageResult = await mediator.SendRequest(
            new AddRecordImageCommand(id, stream)
        );
        return addImageResult.Match(Ok, Problem);
    }

    [HttpGet("{recordId}/image")]
    public async Task<ActionResult<IEnumerable<string>>> GetImages(string recordId) {
        RecordId id = RecordId.Factory.Create(Guid.Parse(recordId));

        var findRecordResult = await mediator.SendRequest(
            new FindRecordByIdQuery(id)
        );

        return findRecordResult.Match(
            record => Ok(record.Images), Problem
        );
    }

    [HttpGet("{recordId}/image/{imageName}")]
    public async Task<ActionResult<string>> GetImage(string recordId, string imageName) {
        RecordId id = RecordId.Factory.Create(Guid.Parse(recordId));

        var findRecordImageResult = await mediator.SendRequest(
            new GetRecordImageUrlQuery(id, imageName)
        );

        return findRecordImageResult.Match(Ok, Problem);
    }

    [HttpGet]
    public async Task<ActionResult<List<SimpleRecordResponse>>> FindRecords() {
        UserId? userId = TryGetUserId();
        if (userId is null) return Problem(Errors.Auth.InvalidCred);

        var findRecordsResult = await mediator.SendRequest(
            new FindRecordsByOwnerIdQuery(userId)
        );
        return findRecordsResult.Match(
            results => Ok(results.Select(MapRecord).ToList()),
            Problem
        );
    }

}
