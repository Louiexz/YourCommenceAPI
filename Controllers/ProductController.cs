using Microsoft.AspNetCore.Mvc;
using WebAPI.models;
using WebAPI.Services.Product;
using WebAPI.Dto.Product;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ProductController(IProductInterface productInterface) : ControllerBase
    {
        private readonly IProductInterface _productInterface = productInterface;
        [HttpGet("GetProducts")]
        public async Task<ActionResult<ResponseModel<List<ProductModel>>>> GetProducts(){
            var products = await _productInterface.GetProducts();
            return Ok(products);
        }
        [HttpGet("GetProduct")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> GetProduct(int Id){
            var product = await _productInterface.GetProduct(Id);
            return Ok(product);
        }
        [HttpPost("CreateProduct")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> CreateProduct(CreateProductDto newProduct){
            var product = await _productInterface.CreateProduct(newProduct);
            return Ok(product);
        }
        [HttpPatch("UpdateProduct")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> UpdateProduct(int Id, UpdateProductDto updateProduct){
            var product = await _productInterface.UpdateProduct(Id, updateProduct);
            return Ok(product);
        }
        [HttpDelete("DeleteProduct")]
        public async Task<ActionResult<ResponseModel<ProductModel>>> DeleteProduct(int Id){
            var product = await _productInterface.DeleteProduct(Id);
            return Ok(product);
        }
    }
}