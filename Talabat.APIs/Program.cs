using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
using Talabat.Core.Entities.Identity;
using Talabat.Repository.Identity;

namespace Talabat.APIs;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Adding DbContext for the database connection
        builder.Services.AddDbContext<StoreContext>(options =>
        {
            options.UseSqlServer(
                builder.Configuration
                    .GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddDbContext<AppIdentityDbContext>(options =>
        {
            options.UseSqlServer(
                builder.Configuration
                    .GetConnectionString("IdentityConnectionString"));
        });

        builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
        {
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { $"{builder.Configuration["Redis:Host"]}:{builder.Configuration["Redis:Port"]}" }
            };

            return ConnectionMultiplexer.Connect(configurationOptions);
        });

        builder.Services.AddApplicationServices();

        builder.Services.AddIdentityServices(builder.Configuration);

        builder.Services.AddCors(options =>
            options.AddPolicy("MyPolicy", options =>
            {
                options.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(builder.Configuration["Cors:AllowedOrigins"].Split(","));
            })
        );

        var app = builder.Build();

        using var scope = app.Services.CreateScope();

        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<StoreContext>();
        var identityDbContext = services.GetRequiredService<AppIdentityDbContext>();

        var loggerFactory = services.GetRequiredService<ILoggerFactory>();

        try
        {
            await dbContext.Database.MigrateAsync(); // Update Database
            await StoreContextSeed.SeedAsync(dbContext); // Seed Data

            await identityDbContext.Database.MigrateAsync(); // Update Database
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            await AppIdentityDbContextSeed.SeedUsersAsync(userManager); // Seed Data
        }
        catch (Exception e)
        {
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(e, "An error occurred while migrating the database.");
            throw;
        }

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseStatusCodePagesWithReExecute("/errors/{0}");
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCors("MyPolicy");
        app.MapControllers();
        app.UseAuthentication();
        app.UseAuthorization();
        app.Run();
    }
}