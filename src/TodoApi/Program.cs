using System.Reflection;
using TodoApi;
using TodoApi.Host;

var appAssembly = Assembly.GetExecutingAssembly();
var builder = WebApplication.CreateBuilder(args);

// Common
builder.Services.AddEfCore(builder.Configuration);

// Host
builder.Services.AddHandlers();
builder.Services.AddBehaviors();
builder.Services.AddSwaggerGen( options =>
{
    options.CustomSchemaIds(x => x.FullName?.Replace("+", ".", StringComparison.Ordinal));
});

builder.Services.AddMediatR(configure => configure.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<ExceptionHandler.KnownExceptionsHandler>();

builder.Services.ConfigureFeatures(builder.Configuration, appAssembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseProductionExceptionHandler();

app.RegisterEndpoints(appAssembly);

app.EnsureDatabaseCreated();

app.Run();