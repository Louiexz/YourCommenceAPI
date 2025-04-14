using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        [HttpGet("categories")]
        public async Task<ActionResult<ResponseModel<List<CategoryModel>>>> GetCategories(){
            var categories = await _categoryInterface.GetCategories();
            return Ok(categories);
        }
        [AllowAnonymous]
        [HttpGet("category")]
        public async Task<ActionResult<ResponseModel<CategoryModel>>> GetCategory(string Id){
            var category = await _categoryInterface.GetCategory(Id);
            return Ok(category);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("category")]
        public async Task<ActionResult<ResponseModel<CategoryModel>>> CreateCategory(CreateCategoryDto newCategory){
            var category = await _categoryInterface.CreateCategory(newCategory);
            return Ok(category);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPatch("category")]
        public async Task<ActionResult<ResponseModel<CategoryModel>>> UpdateCategory(string Id, UpdateCategoryDto updateCategory){
            var category = await _categoryInterface.UpdateCategory(Id, updateCategory);
            return Ok(category);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("category")]
        public async Task<ActionResult<ResponseModel<CategoryModel>>> DeleteCategory(string Id){
            var category = await _categoryInterface.DeleteCategory(Id);
            return Ok(category);
        }
    }
}