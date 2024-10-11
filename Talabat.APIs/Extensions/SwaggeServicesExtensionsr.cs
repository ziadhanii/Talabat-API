namespace Talabat.APIs.Extensions;

public static class SwaggeServicesExtensionsr
{
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        // Configure Swagger/OpenAPI
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }


    public static WebApplication UseSwaggerMiddleWare(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }
}