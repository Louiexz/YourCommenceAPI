using WebAPI.Services.Image;
using WebAPI.Dto.Product;
using WebAPI.models;

namespace WebAPI.View.Product
{
    public class ProductView(IImageInterface imageService) : IProductView
    {
        private readonly IImageInterface _imageService = imageService;

        public async Task<ProductModel> CreateProduct(
            CreateProductDto newProduct, CategoryModel newCategory)
        {
            var ids = _imageService.CreateImages(newProduct.Files);

            return new ProductModel {
                Name = newProduct.Name,
                Description = newProduct.Description,
                Price = newProduct.Price,
                Sale = newProduct.Sale,
                Quantity = newProduct.Quantity,
                Category = new List<CategoryModel> { newCategory },
                ImagesId = await ids
            };
        }
        public async Task<ProductModel> UpdateProduct(
            ProductModel product, UpdateProductDto updateProduct, CategoryModel category)
        {
            foreach (var property in updateProduct.GetType().GetProperties())
            {
                var newValue = property.GetValue(updateProduct);
                if (newValue is string strValue && !string.IsNullOrWhiteSpace(strValue) ||
                        newValue is not string && newValue != null)
                {
                    var productProperty = product.GetType().GetProperty(property.Name);

                    if (productProperty != null && productProperty.CanWrite &&
                        productProperty.Name != "Category" && productProperty.Name != "ImagesId")
                    {
                        productProperty.SetValue(product, newValue);
                    }
                }
            }
            if (updateProduct.Files != null) {
                product.ImagesId = await _imageService.UpdateImages(product.ImagesId, updateProduct.Files);
            }
            product.Category = new List<CategoryModel> { category };
            product.UpdatedAt = DateTime.UtcNow;
            return product;
        }
        public async Task<bool> DeleteProductImages(ProductModel product)
        {
            try {
                await _imageService.DeleteImages(product.ImagesId);
                return true;
            } catch (Exception ex) {
                Console.WriteLine($"Error deleting images: {ex.Message}");
                return false;
            }
        }
    }
}