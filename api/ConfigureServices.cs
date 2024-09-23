using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {

        services.AddHttpContextAccessor();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });

        // services.AddHealthChecks()
        //     .AddDbContextCheck<ApplicationDbContext>();

        // services.AddControllersWithViews(options => { 
        //     options.Filters.Add<ApiExceptionFilterAttribute>();

        //     })
        //     .AddFluentValidation(x => x.AutomaticValidationEnabled = false)
        //     .AddXmlSerializerFormatters()
        //     .AddJsonOptions(options =>
        //     {
        //       options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
        //     });


        // services.AddRazorPages();

        // Customize default API behavior
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);


        // services.AddOpenApiDocument(configure =>
        // {
        //     configure.Title = "gWorks API";
        //     configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
        //     {
        //         Type = OpenApiSecuritySchemeType.ApiKey,
        //         Name = "Authorization",
        //         In = OpenApiSecurityApiKeyLocation.Header,
        //         Description = "Type into the textbox: Bearer {your JWT token}."
        //     });

        //     configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        // });

        // Allow CORS from all Origins
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }
}
