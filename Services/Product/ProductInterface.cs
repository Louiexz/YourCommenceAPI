using WebAPI.models;
using WebAPI.Dto.Product;
using Microsoft.Identity.Client;

namespace WebAPI.Services.Product
{
    public interface IProductInterface
    {
        Task<ResponseModel<GetProductDto>> CreateProduct(CreateProductDto newProduct);
        Task<ResponseModel<List<ProductModel>>> GetCategoryProducts(string Id);
        Task<ResponseModel<List<ProductModel>>> GetProducts();
        Task<ResponseModel<ProductModel>> GetProduct(string Id);
        Task<ResponseModel<List<ProductModel>>> SearchProduct(string searchedProduct);
        Task<ResponseModel<GetProductDto>> UpdateProduct(string Id, UpdateProductDto updateProduct);
        Task<ResponseModel<GetProductDto>> DeleteProduct(string Id);
    }
}
