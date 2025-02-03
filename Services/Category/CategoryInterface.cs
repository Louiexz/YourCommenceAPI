using WebAPI.Dto.Category;
using WebAPI.models;

namespace WebAPI.Services.Category
{
    public interface ICategoryInterface
    {
        Task<ResponseModel<List<CategoryModel>>> GetCategories();
        Task<ResponseModel<CategoryModel>> CreateCategory(CreateCategoryDto newCategory);
        Task<ResponseModel<CategoryModel>> GetCategory(int Id);
        Task<ResponseModel<CategoryModel>> UpdateCategory(int Id, UpdateCategoryDto updateCategory);
        Task<ResponseModel<CategoryModel>> DeleteCategory(int Id);
    }
}