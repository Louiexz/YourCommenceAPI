using MongoDB.Driver;
using WebAPI.Data;

public static class DatabaseServices
{
    public static IServiceCollection AddDatabaseServices(
        this IServiceCollection services, IConfiguration config)
    {   
        // Configuração do MongoDB
        services.AddSingleton<IMongoClient>(s =>
        {
            var connectionString = config["MongoDb:ConnectionString"];
            return new MongoClient(connectionString);
        });
        // Database
        services.AddScoped(s =>
        {
            var client = s.GetRequiredService<IMongoClient>();
            var databaseName = config["MongoDb:DatabaseName"] 
                ?? throw new InvalidOperationException("MongoDb:DatabaseName is not configured.");
            var database = client.GetDatabase(databaseName);
            return database;
        });
        services.AddScoped<AppDbContext>();
        
        return services;
    }
}