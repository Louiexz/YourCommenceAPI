using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using WebAPI.Data;
using WebAPI.models;
using WebAPI.Services.Auth;
using WebAPI.Services.Category;
using WebAPI.Services.Image;
using WebAPI.Services.Product;
using WebAPI.Services.User;
using WebAPI.View.Category;
using WebAPI.View.Product;
using WebAPI.View.User;

public static class ApplicationServices
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services, IConfiguration config)
    {
        // Identity
        services.AddScoped<IPasswordHasher<UserModel>, PasswordHasher<UserModel>>();

        // Interfaces e Serviços
        services.AddScoped<IImageInterface, ImageService>(s =>
        {
            var dbContext = s.GetRequiredService<AppDbContext>();
            return new ImageService(dbContext);
        });
        // Views
        services.AddScoped<IUserView, UserView>(s =>
        {
            var passwordHasher = s.GetRequiredService<IPasswordHasher<UserModel>>();
            return new UserView(passwordHasher);
        });
        services.AddScoped<ICategoryView, CategoryView>(s =>
        {
            var imageService = s.GetRequiredService<IImageInterface>();
            return new CategoryView(imageService);
        });

        services.AddScoped<IProductView, ProductView>(s =>
        {
            var imageService = s.GetRequiredService<IImageInterface>();
            return new ProductView(imageService);
        });
        // Services
        services.AddScoped<IUserInterface, UserService>(s =>
        {
            var dbContext = s.GetRequiredService<AppDbContext>();
            var userView = s.GetRequiredService<IUserView>();
            return new UserService(dbContext, userView);
        });
        services.AddScoped<IProductInterface, ProductService>(s =>
        {
            var dbContext = s.GetRequiredService<AppDbContext>();
            var imageView = s.GetRequiredService<IProductView>();
            return new ProductService(dbContext, imageView);
        });
        services.AddScoped<ICategoryInterface, CategoryService>(s =>
        {
            var dbContext = s.GetRequiredService<AppDbContext>();
            var categoryView = s.GetRequiredService<ICategoryView>();
            return new CategoryService(dbContext, categoryView);
        });
        services.AddScoped<IAuthInterface, AuthService>(s =>
            {
                var dbContext = s.GetRequiredService<AppDbContext>();
                var userView = s.GetRequiredService<IUserView>();
                var cache = s.GetRequiredService<IMemoryCache>();
                return new AuthService(
                    dbContext, config, cache, userView);
            }
        );

        return services;
    }
}