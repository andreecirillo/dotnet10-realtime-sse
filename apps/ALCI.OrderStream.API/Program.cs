using ALCI.OrderStream.API.Extensions;
using ALCI.OrderStream.API.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. SERVICES SETUP
builder.Services.AddOpenApi();
builder.Services.AddSingleton<OrderStreamService>();
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(p => p
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

// 2. PIPELINE SETUP (Middlewares)
app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "ALCI Order Stream API v1");
    options.RoutePrefix = "swagger";
});

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors();

// ENDPOINTS
app.MapOrderEndpoints();

app.Run();