using Microsoft.AspNetCore.Mvc;
using WebAPI.Dto.Category;
using WebAPI.models;
using WebAPI.Services.Category;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryInterface _categoryInterface;

        public CategoryController(ICategoryInterface categoryInterface)
        {
            _categoryInterface = categoryInterface;
        }

        [HttpGet("GetCategories")]
        public async Task<ActionResult<ResponseModel<List<CategoryModel>>>> GetCategories(){
            var categories = await _categoryInterface.GetCategories();
            return Ok(categories);
        }
        [HttpGet("GetCategory")]
        public async Task<ActionResult<ResponseModel<CategoryModel>>> GetCategory(int Id){
            var category = await _categoryInterface.GetCategory(Id);
            return Ok(category);
        }
        [HttpPost("CreateCategory")]
        public async Task<ActionResult<ResponseModel<CategoryModel>>> CreateCategory(CreateCategoryDto newCategory){
            var category = await _categoryInterface.CreateCategory(newCategory);
            return Ok(category);
        }
        [HttpPatch("UpdateCategory")]
        public async Task<ActionResult<ResponseModel<CategoryModel>>> UpdateCategory(int Id, UpdateCategoryDto updateCategory){
            var category = await _categoryInterface.UpdateCategory(Id, updateCategory);
            return Ok(category);
        }
        [HttpDelete("DeleteCategory")]
        public async Task<ActionResult<ResponseModel<CategoryModel>>> DeleteCategory(int Id){
            var category = await _categoryInterface.DeleteCategory(Id);
            return Ok(category);
        }
        
    }
}