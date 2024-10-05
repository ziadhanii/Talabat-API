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

        var app = builder.Build();

        using
            var scope = app.Services.CreateScope();

        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<StoreContext>();


        var loggerFactory = services.GetRequiredService<ILoggerFactory>();

        try
        {
            await dbContext.Database.MigrateAsync(); // Update Database
            await StoreContextSeed.SeedAsync(dbContext);
        }
        catch (Exception e)
        {
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(e, "An error occurred while migrating the database.");
            throw;
        }


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}