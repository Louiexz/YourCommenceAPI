using WebAPI.Data;
using WebAPI.Dto.Product;
using WebAPI.View.Product;
using MongoDB.Driver;
using MongoDB.Bson;
using WebAPI.models;

namespace WebAPI.Services.Product
{
    public class ProductService : IProductInterface
    {
        private readonly AppDbContext _context;
        private readonly IProductView _productView;

        public ProductService(AppDbContext context, IProductView view)
        {
            _context = context;
            _productView = view;
        }

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

                ProductModel product = await _productView.CreateProduct(newProduct, newCategory);

                // Insere um novo objeto ProductModel na coleção de produtos
                await _context.Products.InsertOneAsync(product);

                resposta.Message = "Product created successfully.";
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
            }
            resposta.Status = true;
            return resposta;
        }

        public async Task<ResponseModel<ProductModel>> GetProduct(string Id)
        {
            var resposta = new ResponseModel<ProductModel>();

            try{
                var product = await _context.Products.Find(bankProduct => bankProduct.Id == Id).FirstOrDefaultAsync();

                if (product == null){
                    resposta.Message = $"Product doesn't exist.";    
                    return resposta;
                }
                resposta.Data = product;
                resposta.Message = $"{product.Name} product.";
                resposta.Status = true;
                return resposta;
            }catch(Exception e) {
                resposta.Message = e.Message;
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
                var products = await _context.Products.FindAsync<ProductModel>(filter).Result.ToListAsync();

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
                    resposta.Status = false;
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
                    return resposta;
                }
                product = await _productView.UpdateProduct(product, updateProduct, category);

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
                return resposta;
            }
        }

        public async Task<ResponseModel<GetProductDto>> DeleteProduct(string Id)
        {
            ResponseModel<GetProductDto> resposta = new();
            try{
                var product = await _context.Products.Find(
                    bankProduct => bankProduct.Id == Id).FirstOrDefaultAsync();
                var confirm = await _productView.DeleteProductImages(product);
                if (!confirm) {
                    resposta.Message = $"Error deleting images.";
                    return resposta;
                }
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
                return resposta;
            }
        }
    }
}
