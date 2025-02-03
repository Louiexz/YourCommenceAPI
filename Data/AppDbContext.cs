using MongoDB.Driver;
using WebAPI.models;

namespace WebAPI.Data
{
    public class AppDbContext
    {
        private readonly IMongoDatabase _database;

        public AppDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            _database = client.GetDatabase("your_commence");

            Categories = _database.GetCollection<CategoryModel>("Categories");
            Products = _database.GetCollection<ProductModel>("Products");
            Users = _database.GetCollection<UserModel>("Users");
        }

        public IMongoCollection<CategoryModel> Categories { get; }
        public IMongoCollection<ProductModel> Products { get; }
        public IMongoCollection<UserModel> Users { get; }
    }
}