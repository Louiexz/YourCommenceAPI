using WebAPI.Dto.Category;
using WebAPI.models;

namespace WebAPI.View.Category
{
    public interface ICategoryView
    {
        
        public Task<List<string>> GetImagesId(CreateCategoryDto newCategory);

        public Task<CategoryModel> UpdateCategory(
            CategoryModel category,UpdateCategoryDto updateCategory);
    }
}
