using WebAPI.Services.Image;
using WebAPI.Dto.Product;
using WebAPI.models;

namespace WebAPI.View.Product
{
    public interface IProductView
    {
        public Task<ProductModel> CreateProduct(
            CreateProductDto newProduct, CategoryModel newCategory);
        public Task<ProductModel> UpdateProduct(
            ProductModel product, UpdateProductDto updateProduct, CategoryModel category);
        public Task<bool> DeleteProductImages(ProductModel product);
    }
}