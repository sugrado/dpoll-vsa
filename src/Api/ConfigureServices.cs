using Api.Config;
using Api.Shared.Behaviors.Transaction;
using Api.Shared.Behaviors.Validation;
using Api.Shared.Persistence.Contexts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Api;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            configuration.AddOpenBehavior(typeof(RequestValidationBehavior<,>));
            configuration.AddOpenBehavior(typeof(TransactionScopeBehavior<,>));
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }

    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BaseDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Postgres"));
        });

        return services;
    }

    public static IServiceCollection AddConfigOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<WebApiOptions>(configuration.GetSection(WebApiOptions.WebApi));
        return services;
    }

    public static IServiceCollection AddSecurityServices(this IServiceCollection services, IConfiguration configuration)
    {
        WebApiOptions webApiConfiguration = configuration.GetOptions<WebApiOptions>(WebApiOptions.WebApi);
        services.AddCors(opt =>
            opt.AddDefaultPolicy(p =>
            {
                p.WithOrigins(webApiConfiguration.AllowedOrigins)
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
            })
        );
        return services;
    }
}
