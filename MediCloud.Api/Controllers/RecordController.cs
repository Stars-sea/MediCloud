using MassTransit;
using MassTransit.Mediator;
using MediCloud.Api.Common.Mappers;
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
    public async Task<ActionResult<CreateRecordResponse>> CreateRecord(CreateRecordRequest request) {
        UserId? userId = TryGetUserId();
        if (userId is null) return Problem(Errors.Auth.InvalidCred);

        var createRecordResult = await mediator.SendRequest(request.MapCommand(userId));
        return createRecordResult.Match(r => Ok(r.MapResp()), Problem);
    }

    [HttpGet("{recordId:guid}")]
    public async Task<ActionResult<RecordResponse>> FindRecord(Guid recordId) {
        var findRecordResult = await mediator.SendRequest(recordId.ToFindByIdQuery());
        return findRecordResult.Match(r => Ok(r.MapResp()), Problem);
    }

    [HttpPost("{recordId:guid}/images")]
    public async Task<ActionResult<string>> UploadImage(Guid recordId, IFormFile file) {
        UserId? userId = TryGetUserId();
        if (userId is null) return Problem(Errors.Auth.InvalidCred);

        RecordId id = recordId.ToRecordId();

        var verifyOwnerResult = await mediator.SendRequest(new VerifyRecordOwnerQuery(id, userId));
        if (!verifyOwnerResult.IsSuccess)
            return Problem(verifyOwnerResult.Errors);

        await using var stream         = file.OpenReadStream();
        var             addImageResult = await mediator.SendRequest(new AddRecordImageCommand(id, stream));
        return addImageResult.Match(Ok, Problem);
    }

    [HttpGet("{recordId:guid}/images")]
    public async Task<ActionResult<IEnumerable<string>>> GetImages(Guid recordId) {
        var findRecordResult = await mediator.SendRequest(recordId.ToFindByIdQuery());
        return findRecordResult.Match(record => Ok(record.ImageUrls), Problem);
    }

    [HttpGet("{recordId:guid}/images/{imageName}")]
    public async Task<ActionResult<string>> GetImage(Guid recordId, string imageName) {
        RecordId id = recordId.ToRecordId();

        var findRecordImageResult = await mediator.SendRequest(new GetRecordImageUrlQuery(id, imageName));
        return findRecordImageResult.Match(Ok, Problem);
    }

    [HttpGet]
    public async Task<ActionResult<List<SimpleRecordResponse>>> FindRecords() {
        UserId? userId = TryGetUserId();
        if (userId is null) return Problem(Errors.Auth.InvalidCred);

        var findRecordsResult = await mediator.SendRequest(new FindRecordsByOwnerIdQuery(userId));
        return findRecordsResult.Match(r => Ok(r.MapResps()), Problem);
    }

}
