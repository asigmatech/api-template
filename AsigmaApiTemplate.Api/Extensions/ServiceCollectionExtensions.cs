﻿using AsigmaApiTemplate.Api.Helpers;
using AsigmaApiTemplate.Api.Repositories.GenericRepositories;
using AsigmaApiTemplate.Api.Services.GenericServices;
using AsigmaApiTemplate.Api.Services.Identity;
using AsigmaApiTemplate.Api.Services.Requests;

namespace AsigmaApiTemplate.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services.AddHttpClient<IIdentityHelperService, IdentityHelperService>();
        services.AddScoped<IRequestService, RequestService>();
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