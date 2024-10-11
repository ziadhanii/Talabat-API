namespace Talabat.APIs;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services);
        var app = builder.Build();
        await MigrateDatabase(app);
        ConfigureMiddleware(app);
        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Add services to the container.
        services.AddControllers();
        services.AddSwaggerServices();

        // Adding DbContext for the database connection
        services.AddDbContext<StoreContext>(options =>
            options.UseSqlServer(services.BuildServiceProvider().GetRequiredService<IConfiguration>()
                .GetConnectionString("DefaultConnection")));

        // Add application services
        services.AddApplicationServices();
    }

    private static async Task MigrateDatabase(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<StoreContext>();
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();

        try
        {
            await dbContext.Database.MigrateAsync(); // Update Database
            await StoreContextSeed.SeedAsync(dbContext); // Seed Data
        }
        catch (Exception e)
        {
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(e, "An error occurred while migrating the database.");
            throw;
        }
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerMiddleWare();
        }

        app.UseStatusCodePagesWithReExecute("/errors/{0}");
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAuthorization();
        app.MapControllers();
    }
}