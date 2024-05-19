using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TodoApi.Common.Persistence;

public static class DependencyInjection
{
    public static void AddEfCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor, EventPublisher>();

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        });
    }
    
    public static IApplicationBuilder EnsureDatabaseCreated(this IApplicationBuilder builder)
    {
        using IServiceScope serviceScope = builder.ApplicationServices.CreateScope();

        using AppDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Database.EnsureCreated();
        
        dbContext.SeedDatabase();

        return builder;
    }
}