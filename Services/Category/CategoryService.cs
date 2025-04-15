using MongoDB.Driver;
using WebAPI.Data;
using WebAPI.Dto.Category;
using WebAPI.View.Category;
using WebAPI.models;

namespace WebAPI.Services.Category
{
    public class CategoryService : ICategoryInterface
    {
        private readonly AppDbContext _context;
        private readonly ICategoryView _categoryView;

        public CategoryService(AppDbContext context, ICategoryView view)
        {
            _context = context;
            _categoryView = view;
        }

        public async Task<ResponseModel<CategoryModel>> CreateCategory(CreateCategoryDto newCategory)
        {
            var resposta = new ResponseModel<CategoryModel>();
            try{

                var checkCategory = await _context.Categories
                    .Find(bankCategory => bankCategory.Name == newCategory.Name).FirstOrDefaultAsync();

                if (checkCategory != null)
                {
                    resposta.Message = $"Category already exist.";
                    return resposta;
                }

                var ids = await _categoryView.GetImagesId(newCategory);
                var category = new CategoryModel
                {
                    Name = newCategory.Name,
                    Description = newCategory.Description,
                    ImagesId = ids,
                };
                await _context.Categories.InsertOneAsync(category);

                resposta.Message = $"Category created successfully.";
                resposta.Status = true;
                return resposta;
            }catch(Exception e) {
                resposta.Message = e.Message;
                return resposta;
            }
        }

        public async Task<ResponseModel<CategoryModel>> DeleteCategory(string Id)
        {
            var resposta = new ResponseModel<CategoryModel>();
            try{
                var category = await _context.Categories.DeleteOneAsync(
                    bankCategory => bankCategory.Id == Id);

                if (category.DeletedCount == 0){
                    resposta.Message = $"Category doesn't exist.";    
                    return resposta;
                }
                resposta.Message = $"Category deleted successfully.";
                resposta.Status = true;
                return resposta;
            }catch(Exception e) {
                resposta.Message = e.Message;
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
                resposta.Status = true;
                return resposta;
            }
            catch(Exception e) {
                resposta.Message = e.Message;
                return resposta;
            }
        }

        public async Task<ResponseModel<CategoryModel>> GetCategory(string Id)
        {
            var resposta = new ResponseModel<CategoryModel>();
            try{
                var category = await _context.Categories.Find(bankCategory => bankCategory.Id == Id).FirstOrDefaultAsync();

                if (category == null){
                    resposta.Message = $"Category doesn't exist.";    
                    return resposta;
                }
                resposta.Data = category;
                resposta.Message = $"Products of {category.Name} category.";
                resposta.Status = true;
                return resposta;
            }catch(Exception e) {
                resposta.Message = e.Message;
                return resposta;
            }
        }

        public async Task<ResponseModel<CategoryModel>> UpdateCategory(string Id, UpdateCategoryDto toUpdateCategory)
        {
            var resposta = new ResponseModel<CategoryModel>();
            try{
                var category = await _context.Categories.Find(
                    bankCategory => bankCategory.Id == Id).FirstOrDefaultAsync();

                if (category == null)
                {
                    resposta.Message = $"Category doesn't exist.";
                    return resposta;
                }

                category = await _categoryView.UpdateCategory(category, toUpdateCategory);
                await _context.Categories.ReplaceOneAsync(
                    bankCategory => bankCategory.Id == Id, category);

                resposta.Message = $"Category updated successfully.";
                resposta.Status = true;
                return resposta;
            }catch(Exception e) {
                resposta.Message = e.Message;
                return resposta;
            }
        }
    }
}
