using WebAPI.Dto.Category;
using WebAPI.models;

namespace WebAPI.Services.Category
{
    public interface ICategoryInterface
    {
        Task<ResponseModel<CategoryModel>> CreateCategory(CreateCategoryDto newCategory);
        Task<ResponseModel<CategoryModel>> GetCategory(string Id);
        Task<ResponseModel<List<CategoryModel>>> GetCategories();
        Task<ResponseModel<CategoryModel>> UpdateCategory(string Id, UpdateCategoryDto updateCategory);
        Task<ResponseModel<CategoryModel>> DeleteCategory(string Id);
    }
}
