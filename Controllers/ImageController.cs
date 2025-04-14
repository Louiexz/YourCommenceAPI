using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Services.Image;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageInterface _imageInterface;

        public ImageController(IImageInterface imageInterface)
        {
            _imageInterface = imageInterface;
        }
        [AllowAnonymous]
        [HttpGet("GetImage")]
        public async Task<IActionResult> GetImage(string Id)
        {
            var image = await _imageInterface.GetImage(Id);

            if (image is FileStreamResult fileStream)
            {
                return fileStream;
            }
            else if (image is ObjectResult objectResult)
            {
                return objectResult;
            }
            else if (image is NotFoundResult notFoundResult)
            {
                return notFoundResult;
            }
            else
            {
            return BadRequest("Unexpected result type");
            }
        }
    }
}