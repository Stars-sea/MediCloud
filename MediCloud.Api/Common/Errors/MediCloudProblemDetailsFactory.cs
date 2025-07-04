using System.Diagnostics;
using ErrorOr;
using MediCloud.Api.Common.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace MediCloud.Api.Common.Errors;

public class MediCloudProblemDetailsFactory(
    IOptions<ApiBehaviorOptions>?    options,
    IOptions<ProblemDetailsOptions>? problemDetailsOptions = null
) : ProblemDetailsFactory {
    
    private readonly ApiBehaviorOptions?            _options   = options?.Value;
    private readonly Action<ProblemDetailsContext>? _configure = problemDetailsOptions?.Value.CustomizeProblemDetails;

    public override ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        int?        statusCode = null,
        string?     title      = null,
        string?     type       = null,
        string?     detail     = null,
        string?     instance   = null
    ) {
        statusCode ??= 500;
        ProblemDetails problemDetails = new() {
            Status   = statusCode,
            Title    = title,
            Type     = type,
            Detail   = detail,
            Instance = instance
        };
        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);
        return problemDetails;
    }

    public override ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext          httpContext,
        ModelStateDictionary modelStateDictionary,
        int?                 statusCode = null,
        string?              title      = null,
        string?              type       = null,
        string?              detail     = null,
        string?              instance   = null
    ) {
        ArgumentNullException.ThrowIfNull(modelStateDictionary, nameof(modelStateDictionary));
        statusCode ??= 400;
        
        ValidationProblemDetails validationProblemDetails = new(modelStateDictionary) {
            Status   = statusCode,
            Type     = type,
            Detail   = detail,
            Instance = instance
        };

        if (title != null)
            validationProblemDetails.Title = title;
        ApplyProblemDetailsDefaults(httpContext, validationProblemDetails, statusCode.Value);
        return validationProblemDetails;
    }
    
    private void ApplyProblemDetailsDefaults(
        HttpContext    httpContext,
        ProblemDetails problemDetails,
        int            statusCode
    ) {
        problemDetails.Status ??= statusCode;

        if (_options!.ClientErrorMapping.TryGetValue(statusCode, out ClientErrorData? clientErrorData)) {
            problemDetails.Title ??= clientErrorData.Title;
            problemDetails.Type  ??= clientErrorData.Link;
        }

        string traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        problemDetails.Extensions["traceId"] = traceId;

        if (httpContext.Items[HttpContextItemKeys.Errors] is List<Error> errors)
            problemDetails.Extensions.Add("errorCodes", errors.Select(e => e.Code));

        _configure?.Invoke(new ProblemDetailsContext {
                HttpContext    = httpContext,
                ProblemDetails = problemDetails
            }
        );
    }
}
