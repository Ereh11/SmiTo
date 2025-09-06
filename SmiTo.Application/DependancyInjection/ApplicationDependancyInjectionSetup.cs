using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using SmiTo.Application.Interfaces;
using SmiTo.Application.Services;
using SmiTo.Application.Validators;

namespace SmiTo.Application.DependancyInjection;

public static class ApplicationDependancyInjectionSetup
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IURLService, URLService>();

        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
        //services.AddScoped<IVisitService, VisitService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IShortCodeGenerator, ShortCodeGenerator>();
        //.AddScoped<IAnalyticsService, AnalyticsService>();
    }
}
