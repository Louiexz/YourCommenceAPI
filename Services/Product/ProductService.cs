using WebAPI.models;
using WebAPI.Data;
using MongoDB.Driver;
using WebAPI.Dto.Product;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.Services.Product
{
    public class ProductService : IProductInterface
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<ProductModel>> CreateProduct(CreateProductDto newProduct)
        {
            
            ResponseModel<ProductModel> resposta = new();
            try
            {
                var category = await _context.Categories
                    .Find(bankCategory => bankCategory.Id == newProduct.Category).FirstOrDefaultAsync();

                if (category == null){
                    resposta.Message = $"Category don't exist.";    
                    return resposta;
                }

                var product = new ProductModel {
                    Name = newProduct.Name,
                    Description = newProduct.Description,
                    Price = newProduct.Price,
                    Sale = newProduct.Sale,
                    Quantity = newProduct.Quantity,
                    Category = category,
                };
                // Insere um novo objeto ProductModel na coleção de produtos
                await _context.Products.InsertOneAsync(product);

                resposta.Message = "Product created successfully.";
                resposta.Status = true;
                resposta.Data = product;
            }
            catch (Exception e)
            {
                resposta.Message = e.Message;
                resposta.Status = false;
            }
            return resposta;
        }

        public async Task<ResponseModel<ProductModel>> DeleteProduct(int Id)
        {
            ResponseModel<ProductModel> resposta = new();
            try{
                var product = await _context.Products.DeleteOneAsync(bankProduct => bankProduct.Id == Id);

                if (product.DeletedCount == 0){
                    resposta.Message = $"Product doesn't exist.";    
                    return resposta;
                }
                resposta.Message = $"Product deleted sucessfully.";
                return resposta;
            }catch(Exception e) {
                resposta.Message = e.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<ProductModel>> GetProduct(int Id)
        {
            ResponseModel<ProductModel> resposta = new();
            try{
                var product = await _context.Products.Find(bankCategory => bankCategory.Id == Id).FirstOrDefaultAsync();

                if (product == null){
                    resposta.Message = $"Product don't exist.";    
                    return resposta;
                }
                resposta.Data = product;
                resposta.Message = $"{product.Name} product.";
                return resposta;
            }catch(Exception e) {
                resposta.Message = e.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<ProductModel>>> GetProducts()
        {
            ResponseModel<List<ProductModel>> resposta = new();
            try{
                var products = await _context.Products.Find(_ => true).ToListAsync();

                resposta.Data = products;
                resposta.Message = "All products.";
                return resposta;
            }
            catch(Exception e) {
                resposta.Message = e.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<ProductModel>> UpdateProduct(int Id, UpdateProductDto updateProduct)
        {
            ResponseModel<ProductModel> resposta = new();
            try{
                var category = await _context.Categories
                    .Find(bankCategory => bankCategory.Id == updateProduct.Category).FirstOrDefaultAsync();

                if (category == null)
                {
                    resposta.Message = $"Category doesn't exist.";
                    return resposta;
                }

                var product = await _context.Products.Find(bankProduct => bankProduct.Id == Id).FirstOrDefaultAsync();

                if (product == null)
                {
                    resposta.Message = $"Product doesn't exist.";
                    return resposta;
                }

                foreach (var property in updateProduct.GetType().GetProperties())
                {
                    var newValue = property.GetValue(updateProduct);
                    if (newValue != null && newValue.ToString() != "string" && newValue.ToString() != "")
                    {
                        var productProperty = product.GetType().GetProperty(property.Name);
                        if (productProperty != null && productProperty.CanWrite && productProperty.Name != "Category")
                        {
                            productProperty.SetValue(product, newValue);
                        }
                    }
                }
                product.Category = category;
                product.UpdatedAt = DateTime.UtcNow;
                await _context.Products.ReplaceOneAsync(bankProduct => bankProduct.Id == Id, product);

                resposta.Message = $"Product updated sucessfully.";
                return resposta;
            }catch(Exception e) {
                resposta.Message = e.Message;
                resposta.Status = false;
                return resposta;
            }
        }
    }
}