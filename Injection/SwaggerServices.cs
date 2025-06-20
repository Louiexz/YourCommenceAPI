using Microsoft.OpenApi.Models;

public static class SwaggerGen
{
    public static IServiceCollection AddSwaggerGenServices(
        this IServiceCollection services, WebApplicationBuilder builder)
    {
        // Adicione o Swagger
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "YourCommence",
                Description = "API para o YourCommence, voltado para estudos e aprendizado.",
                Contact = new OpenApiContact
                {
                    Name = builder.Configuration["Name"],
                    Email = builder.Configuration["Email"]
                }
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Insira 'Bearer' seguido de um espaço e o token JWT."
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        return services;
    }
}