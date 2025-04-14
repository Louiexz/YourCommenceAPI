using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebAPI.Services.Product;
using WebAPI.Services.User;
using WebAPI.Services.Category;
using WebAPI.Services.Image;
using WebAPI.Services.Auth;
using MongoDB.Driver;
using WebAPI.Data;
using System.Text;
using Microsoft.AspNetCore.Identity;
using WebAPI.models;

var builder = WebApplication.CreateBuilder(args);

// Adicione serviços ao contêiner.
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
// Adicione configuração de variáveis de ambiente
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "YourCommence",
        Description = "API para o YourCommence, voltado para estudos e aprendizado.",
        Contact = new OpenApiContact
        {
            Name = "Luiz Augusto",
            Email = "luiz.lima.developer@gmail.com"
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

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80); // Escuta na porta 80
});

// Adicione serviços de autenticação e autorização
string secretKey = builder.Configuration["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("SecretKey is not configured.");
var issuer = builder.Configuration["JwtSettings:Issuer"] ?? throw new InvalidOperationException("Issuer is not configured.");
var audience = builder.Configuration["JwtSettings:Audience"] ?? throw new InvalidOperationException("Audience is not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("UserType", "Admin"));
});

// Configuração do MongoDB
builder.Services.AddSingleton<IMongoClient>(s =>
{
    var connectionString = builder.Configuration["MongoDb:ConnectionString"];
    return new MongoClient(connectionString);
});

builder.Services.AddScoped(s =>
{
    var client = s.GetRequiredService<IMongoClient>();
    var databaseName = builder.Configuration["MongoDb:DatabaseName"] 
        ?? throw new InvalidOperationException("MongoDb:DatabaseName is not configured.");
    var database = client.GetDatabase(databaseName);
    return database;
});

builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped<IPasswordHasher<UserModel>, PasswordHasher<UserModel>>();

builder.Services.AddScoped<IAuthInterface, AuthService>();
builder.Services.AddScoped<IImageInterface, ImageService>();
builder.Services.AddScoped<IUserInterface, UserService>();

// Fix for ProductService and CategoryService
builder.Services.AddScoped<IProductInterface, ProductService>(provider =>
{
    var context = provider.GetRequiredService<AppDbContext>();
    var imageService = provider.GetRequiredService<IImageInterface>();
    return new ProductService(context, imageService as ImageService ?? throw new InvalidCastException("Failed to cast IImageInterface to ImageService."));
});

builder.Services.AddScoped<ICategoryInterface, CategoryService>(provider =>
{
    var context = provider.GetRequiredService<AppDbContext>();
    var imageService = provider.GetRequiredService<IImageInterface>();
    return new CategoryService(context, imageService as ImageService ?? throw new InvalidCastException("Failed to cast IImageInterface to ImageService."));
});

var app = builder.Build();

// Configure o pipeline de requisição HTTP.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API V1");
        c.RoutePrefix = ""; // <- opcional, se quiser que carregue em `/`
    });
}


app.UseHttpsRedirection();

app.UseAuthentication(); // <- antes de UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
