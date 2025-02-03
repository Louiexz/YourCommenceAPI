using WebAPI.models;
using WebAPI.Dto.Product;

namespace WebAPI.Services.Product
{
    public interface IProductInterface
    {
        Task<ResponseModel<List<ProductModel>>> GetProducts();
        Task<ResponseModel<ProductModel>> CreateProduct(CreateProductDto newProduct);
        Task<ResponseModel<ProductModel>> GetProduct(int Id);
        Task<ResponseModel<ProductModel>> UpdateProduct(int Id, UpdateProductDto updateProduct);
        Task<ResponseModel<ProductModel>> DeleteProduct(int Id);
    }
}