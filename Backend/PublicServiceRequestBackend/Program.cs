using Microsoft.EntityFrameworkCore;
using PublicServiceRequestBackend.Data;
using PublicServiceRequestBackend.Middleware;
using PublicServiceRequestBackend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<PublicServiceRequestDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IPublicServiceRequest, PublicServiceRequestService>();

var app = builder.Build();

async Task MigrateDatabaseAsync()
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<PublicServiceRequestDbContext>();
        await context.Database.MigrateAsync();
        Console.WriteLine("Database migration completed successfully.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
        throw;
    }
}

await MigrateDatabaseAsync();

app.MapOpenApi();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseRouting();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();