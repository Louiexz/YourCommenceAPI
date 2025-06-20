using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Services.Product;
using WebAPI.Dto.Product;
using WebAPI.models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ProductController(IProductInterface productInterface) : ControllerBase
    {
        private readonly IProductInterface _productInterface = productInterface;

        [AllowAnonymous]
        [HttpGet("products")]
        public async Task<ActionResult<ResponseModel<List<GetProductDto>>>> GetProducts(){
            var products = await _productInterface.GetProducts();
            return Ok(products);
        }
        [AllowAnonymous]
        [HttpGet("categoryproducts")]
        public async Task<ActionResult<ResponseModel<List<GetProductDto>>>> GetCategoryProducts(string Id){
            var products = await _productInterface.GetCategoryProducts(Id);
            return Ok(products);
        }
        [AllowAnonymous]
        [HttpGet("product")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> GetProduct(string Id){
            var product = await _productInterface.GetProduct(Id);
            return Ok(product);
        }
        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<ActionResult<ResponseModel<List<GetProductDto>>>> SearchProduct(string searchedProduct){
            var products = await _productInterface.SearchProduct(searchedProduct);
            return Ok(products);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("product")]
        public async Task<ActionResult<ResponseModel<GetProductDto>>> CreateProduct(CreateProductDto newProduct){
            var product = await _productInterface.CreateProduct(newProduct);
            return Ok(product);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPatch("product")]
        public async Task<ActionResult<ResponseModel<GetProductDto>>> UpdateProduct(string Id, UpdateProductDto updateProduct){
            var product = await _productInterface.UpdateProduct(Id, updateProduct);
            return Ok(product);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("product")]
        public async Task<ActionResult<ResponseModel<GetProductDto>>> DeleteProduct(string Id){
            var product = await _productInterface.DeleteProduct(Id);
            return Ok(product);
        }
    }
}
