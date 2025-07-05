using MediCloud.Api;
using MediCloud.Api.Common.Errors;
using MediCloud.Application;
using MediCloud.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services
       .AddApplication()
       .AddInfrastructure(builder.Configuration);

builder.Services.AddIdentity();
builder.Services.AddControllers();

builder.Services.AddSingleton<ProblemDetailsFactory, MediCloudProblemDetailsFactory>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi("/openapi/openapi.json");

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
