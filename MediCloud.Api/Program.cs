using MediCloud.Api;
using MediCloud.Application;
using MediCloud.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services
       .AddPresentation()
       .AddIdentity()
       .AddApplication()
       .AddInfrastructure(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi("/openapi/openapi.json");

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
