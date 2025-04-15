using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

public static class AuthServices
{
    public static IServiceCollection AddAuthServices(
        this IServiceCollection services, WebApplicationBuilder builder)
    {
        // Adicione serviços de autenticação e autorização
        string secretKey = builder.Configuration["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("SecretKey is not configured.");
        var issuer = builder.Configuration["JwtSettings:Issuer"] ?? throw new InvalidOperationException("Issuer is not configured.");
        var audience = builder.Configuration["JwtSettings:Audience"] ?? throw new InvalidOperationException("Audience is not configured.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy("AdminOnly", policy =>
                policy.RequireClaim("UserType", "Admin"));

        return services;
    }
}