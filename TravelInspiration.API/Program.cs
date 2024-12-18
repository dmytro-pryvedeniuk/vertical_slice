using TravelInspiration.API;
using TravelInspiration.API.Shared.Slices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddProblemDetails();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer();

builder.Services.RegisterApplicationServices();
builder.Services.RegisterPersistenceServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
}
app.UseStatusCodePages();

app.UseAuthentication();
app.UseAuthorization();
app.MapSliceEndpoints();

app.Run();

public partial class Program { }