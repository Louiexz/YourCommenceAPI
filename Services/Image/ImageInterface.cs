using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Services.Image
{
    public interface IImageInterface
    {
        Task<IActionResult> GetImage(string Id);
        Task<List<string>> CreateImages(List<IFormFile> images);
        Task<List<string>> UpdateImages(List<string> Ids, List<IFormFile> updateImages);
        Task DeleteImages(List<string> Ids);
    }
}
