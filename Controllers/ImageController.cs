using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bloggie.Repositories;
using System.Net;

namespace Bloggie.Controllers
{
    //here i used a controller of API-Controller empty that is using for uploading  the image 
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            var imageUrl = await imageRepository.UploadAsync(file);

            if (string.IsNullOrEmpty(imageUrl))
            {
                return Problem("Something went wrong!", null, (int)HttpStatusCode.InternalServerError);
            }

            return new JsonResult(new { link = imageUrl });
        }

    }
}
