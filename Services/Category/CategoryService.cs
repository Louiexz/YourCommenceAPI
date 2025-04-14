using MongoDB.Driver;
using WebAPI.Data;
using WebAPI.models;
using WebAPI.Dto.Category;
using WebAPI.Services.Image;

namespace WebAPI.Services.Category
{
    public class CategoryService(
        AppDbContext context, ImageService imageService) : ICategoryInterface
    {
        private readonly AppDbContext _context = context;
        private readonly ImageService _imageService = imageService;

        public async Task<ResponseModel<CategoryModel>> CreateCategory(CreateCategoryDto newCategory)
        {
            ResponseModel<CategoryModel> resposta = new();
            try{

                CategoryModel checkCategory = await _context.Categories
                    .Find(bankCategory => bankCategory.Name == newCategory.Name).FirstOrDefaultAsync();

                if (checkCategory != null)
                {
                    resposta.Message = $"Category already exist.";
                    return resposta;
                }

                var ids = _imageService.CreateImages(newCategory.Files);

                var category = new CategoryModel {
                    Name = newCategory.Name,
                    Description = newCategory.Description,
                    ImagesId = await ids,
                };
                await _context.Categories.InsertOneAsync(category);

                resposta.Message = $"Category created sucessfully.";
                return resposta;
            }catch(Exception e) {
                resposta.Message = e.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<CategoryModel>> DeleteCategory(string Id)
        {
            ResponseModel<CategoryModel> resposta = new ResponseModel<CategoryModel>();
            try{
                var category = await _context.Categories.DeleteOneAsync(bankCategory => bankCategory.Id == Id);

                if (category.DeletedCount == 0){
                    resposta.Message = $"Category don't exist.";    
                    return resposta;
                }
                resposta.Message = $"Category deleted sucessfully.";
                return resposta;
            }catch(Exception e) {
                resposta.Message = e.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<List<CategoryModel>>> GetCategories()
        {
            ResponseModel<List<CategoryModel>> resposta = new();
            try{
                var categories = await _context.Categories.Find(_ => true).ToListAsync();
                resposta.Data = categories;
                resposta.Message = "All categories.";
                return resposta;
            }
            catch(Exception e) {
                resposta.Message = e.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<CategoryModel>> GetCategory(string Id)
        {
            ResponseModel<CategoryModel> resposta = new ResponseModel<CategoryModel>();
            try{
                var category = await _context.Categories.Find(bankCategory => bankCategory.Id == Id).FirstOrDefaultAsync();

                if (category == null){
                    resposta.Message = $"Category don't exist.";    
                    return resposta;
                }
                resposta.Data = category;
                resposta.Message = $"Products of {category.Name} category.";
                return resposta;
            }catch(Exception e) {
                resposta.Message = e.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<CategoryModel>> UpdateCategory(string Id, UpdateCategoryDto updateCategory)
        {
            ResponseModel<CategoryModel> resposta = new ResponseModel<CategoryModel>();
            try{
                var category = await _context.Categories.Find(bankCategory => bankCategory.Id == Id).FirstOrDefaultAsync();

                if (category == null)
                {
                    resposta.Message = $"Category doesn't exist.";
                    return resposta;
                }

                foreach (var property in updateCategory.GetType().GetProperties())
                {
                    var newValue = property.GetValue(updateCategory);
                    if (newValue != null)
                    {
                        var categoryProperty = category.GetType().GetProperty(property.Name);
                        if (categoryProperty != null && categoryProperty.CanWrite)
                        {
                            categoryProperty.SetValue(category, newValue);
                        }
                    }
                }
                category.UpdatedAt = DateTime.UtcNow;
                await _context.Categories.ReplaceOneAsync(bankCategory => bankCategory.Id == Id, category);

                resposta.Message = $"Category updated sucessfully.";
                return resposta;
            }catch(Exception e) {
                resposta.Message = e.Message;
                resposta.Status = false;
                return resposta;
            }
        }
    }
}
