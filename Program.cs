using WebAPI.Services.Product;
using WebAPI.Services.User;
using WebAPI.Services.Category;
using MongoDB.Driver;
using WebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Adicione serviços ao contêiner.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adicione serviços de autenticação e autorização
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://localhost:5056";
        options.RequireHttpsMetadata = false;
        options.Audience = "api1";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "api1");
    });
});
// Adicione configuração de variáveis de ambiente
builder.Configuration.AddEnvironmentVariables();

// Configuração do MongoDB
builder.Services.AddSingleton<IMongoClient>(s =>
{
    var configuration = s.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("MongoDb");
    return new MongoClient(connectionString);
});

builder.Services.AddScoped(s =>
{
    var client = s.GetRequiredService<IMongoClient>();
    var database = client.GetDatabase("your_commence");
    return database;
});

builder.Services.AddScoped<AppDbContext>();

builder.Services.AddScoped<IProductInterface, ProductService>();
builder.Services.AddScoped<IUserInterface, UserService>();
builder.Services.AddScoped<ICategoryInterface, CategoryService>();

var app = builder.Build();

// Configure o pipeline de requisição HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();