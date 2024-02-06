using AsigmaApiTemplate.Api.Repositories.GenericRepositories;
using AsigmaApiTemplate.Api.Services.GenericServices;

namespace AsigmaApiTemplate.Api.Extensions;

public static class ServiceCollectionExtensions
{
    // public static IServiceCollection AddAutoMapperExtension<T>(this IServiceCollection services) where T : Profile
    // {
    //     services.AddAutoMapper(typeof(T));
    //     return services;
    // }
    
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.Scan(scan => scan
                
            .FromAssemblyOf<Program>()
            
            .AddClasses(classes => classes.AssignableTo(typeof(IGenericRepository<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime()

            .AddClasses(classes => classes.AssignableTo(typeof(IGenericService<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return services;
    }

}

