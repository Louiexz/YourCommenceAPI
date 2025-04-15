var builder = WebApplication.CreateBuilder(args);

// Adicione serviços ao contêiner.
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
// Adicione configuração de variáveis de ambiente
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSwaggerGenServices(builder);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80); // Escuta na porta 80
});
builder.Services.AddAuthServices(builder);

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
