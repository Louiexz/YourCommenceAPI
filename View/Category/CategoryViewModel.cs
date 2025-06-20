using WebAPI.Services.Image;
using WebAPI.Dto.Category;
using WebAPI.models;

namespace WebAPI.View.Category
{
    public class CategoryView(IImageInterface imageService) : ICategoryView
    {
        private readonly IImageInterface _imageService = imageService;

        public async Task<List<string>> GetImagesId(CreateCategoryDto newCategory)
        {
            var ids = await _imageService.CreateImages(newCategory.Files);

            return ids;
        }

        public async Task<CategoryModel> UpdateCategory(
            CategoryModel category,
            UpdateCategoryDto updateCategory)
        {
            foreach (var property in updateCategory.GetType().GetProperties())
            {
                var newValue = property.GetValue(updateCategory);
                if (newValue != null)
                {
                    var categoryProperty = category.GetType().GetProperty(property.Name);
                    if (categoryProperty != null && categoryProperty.CanWrite
                        && categoryProperty.Name != "ImagesId")
                    {
                        categoryProperty.SetValue(category, newValue);
                    }
                }
            }

            if (updateCategory.Files.Count > 0)
            {
                category.ImagesId = await _imageService.UpdateImages(
                    category.ImagesId, updateCategory.Files);
            }

            category.UpdatedAt = DateTime.UtcNow;

            return category;
        }
    }
}
