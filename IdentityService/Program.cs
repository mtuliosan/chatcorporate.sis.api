using IdentityService;
using IdentityService.Domain.Config;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Environment);

startup.ConfigureServices(builder.Services);

var app = builder.Build();
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();


startup.Configure(app, provider);

app.Run();
