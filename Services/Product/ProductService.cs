using WebAPI.models;
using WebAPI.Data;
using MongoDB.Driver;
using WebAPI.Dto.Product;
using WebAPI.Services.Image;
using MongoDB.Bson;

namespace WebAPI.Services.Product
{
    public class ProductService(
        AppDbContext context, ImageService imageService) : IProductInterface
    {
        private readonly AppDbContext _context = context;
        private readonly ImageService _imageService = imageService;

        public async Task<ResponseModel<GetProductDto>> CreateProduct(CreateProductDto newProduct)
        {
            ResponseModel<GetProductDto> resposta = new();
            try
            {
                var categoryName = newProduct.Category;

                CategoryModel newCategory = await _context.Categories
                    .Find(bankCategory => bankCategory.Name == categoryName).FirstOrDefaultAsync();

                if (newCategory == null)
                {
                    resposta.Message = $"Category doesn't exist.";
                    return resposta;
                }

                var existingProduct = await _context.Products
                    .Find(p => p.Name == newProduct.Name && p.Category.Any(c => c.Id == newCategory.Id))
                    .FirstOrDefaultAsync();

                if (existingProduct != null)
                {
                    resposta.Message = $"Product with the same name already exists in this category.";
                    return resposta;
                }

                var ids = _imageService.CreateImages(newProduct.Files);

                var product = new ProductModel {
                    Name = newProduct.Name,
                    Description = newProduct.Description,
                    Price = newProduct.Price,
                    Sale = newProduct.Sale,
                    Quantity = newProduct.Quantity,
                    Category = new List<CategoryModel> { newCategory },
                    ImagesId = await ids
                };
                // Insere um novo objeto ProductModel na coleção de produtos
                await _context.Products.InsertOneAsync(product);

                resposta.Message = "Product created successfully.";
                resposta.Status = true;
                resposta.Data = new GetProductDto
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Sale = product.Sale,
                    Quantity = product.Quantity,
                    Category = product.Category[0].Name,
                    ImagesId = product.ImagesId
                };
            }
            catch (Exception)
            {
                resposta.Message = "An unexpected error occurred while creating the product.";
                resposta.Status = false;
            }
            return resposta;
        }

        public async Task<ResponseModel<ProductModel>> GetProduct(string Id)
        {
            ResponseModel<ProductModel> resposta = new();

            try{
                var product = await _context.Products.Find(bankProduct => bankProduct.Id == Id).FirstOrDefaultAsync();

                if (product == null){
                    resposta.Message = $"Product doesn't exist.";    
                    return resposta;
                }
                resposta.Data = product;
                resposta.Status = true;
                resposta.Message = $"{product.Name} product.";
                return resposta;
            }catch(Exception e) {
                resposta.Message = e.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<ProductModel>>> SearchProduct(string searchedProduct)
        {
            ResponseModel<List<ProductModel>> resposta = new();
            try{
                var filter = Builders<ProductModel>.Filter.Or(
                    Builders<ProductModel>.Filter.Regex(p => p.Name, new BsonRegularExpression(searchedProduct, "i")),
                    Builders<ProductModel>.Filter.Regex(p => p.Description, new BsonRegularExpression(searchedProduct, "i"))
                );
                var products = await _context.Products.Find(filter).ToListAsync();

                if (products == null  || products.Count == 0){
                    resposta.Message = $"Category doesn't exist.";    
                    return resposta;
                }

                resposta.Data = products;
                resposta.Message = $"{searchedProduct} products.";
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

        public async Task<ResponseModel<List<ProductModel>>> GetCategoryProducts(string Id)
        {
            ResponseModel<List<ProductModel>> resposta = new();
            try{
                var products = await _context.Products.Find(bankCategory => bankCategory.Category.Any(c => c.Id == Id)).ToListAsync();
                

                if (products == null  || products.Count == 0){
                    resposta.Message = $"Category doesn't exist.";    
                    return resposta;
                }

                resposta.Data = products;
                resposta.Message = "All category products.";
                return resposta;
            }
            catch(Exception e) {
                resposta.Message = e.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<GetProductDto>> UpdateProduct(string Id, UpdateProductDto updateProduct)
        {
            ResponseModel<GetProductDto> resposta = new();
            try{
                var category = await _context.Categories
                    .Find(bankCategory => bankCategory.Name == updateProduct.Category).FirstOrDefaultAsync();

                var product = await _context.Products
                    .Find(bankProduct => bankProduct.Id == Id).FirstOrDefaultAsync();

                if (product == null)
                {
                    resposta.Message = $"Product doesn't exist.";
                    return resposta;
                }

                if (category == null)
                {
                    resposta.Message = $"No category found with name '{updateProduct.Category}'.";
                    resposta.Status = false;
                    return resposta;
                }
                
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
                await _context.Products.ReplaceOneAsync(bankProduct => bankProduct.Id == Id, product);

                resposta.Message = $"Product updated sucessfully.";
                resposta.Status = true;
                resposta.Data = new GetProductDto
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Sale = product.Sale,
                    Quantity = product.Quantity,
                    Category = product.Category[0].Name,
                    ImagesId = product.ImagesId
                };
                return resposta;
            }catch(Exception e) {
                resposta.Message = e.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<GetProductDto>> DeleteProduct(string Id)
        {
            ResponseModel<GetProductDto> resposta = new();
            try{
                var product = await _context.Products.Find(bankProduct => bankProduct.Id == Id).FirstOrDefaultAsync();
                await _imageService.DeleteImages(product.ImagesId);
                var deletedProduct = await _context.Products.DeleteOneAsync(bankProduct => bankProduct.Id == Id);

                if (deletedProduct.DeletedCount == 0){
                    resposta.Message = $"Product doesn't exist.";    
                    return resposta;
                }
                resposta.Message = $"Product deleted sucessfully.";
                resposta.Status = true;
                resposta.Data = new GetProductDto {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Sale = product.Sale,
                    Quantity = product.Quantity,
                    Category = product.Category[0].Name,
                    ImagesId = product.ImagesId
                };
                return resposta;
            }catch(Exception e) {
                resposta.Message = e.Message;
                resposta.Status = false;
                return resposta;
            }
        }
    }
}
