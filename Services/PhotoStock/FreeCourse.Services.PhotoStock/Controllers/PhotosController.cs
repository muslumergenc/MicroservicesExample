using FreeCourse.Services.PhotoStock.Dtos;
using FreeCourse.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController
    {
        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo,CancellationToken cancellationToken)
        {
            if (photo!=null && photo.Length>0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);
                using var stream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(stream, cancellationToken);
                var returnPath = "photos/" + photo.FileName;
                PhotoDto photoDto = new()
                {
                    Url = returnPath
                };
                return CreateActionResultInstance(Shared.Dtos.Response<PhotoDto>.Success(photoDto, 200));
            }

            return CreateActionResultInstance(Shared.Dtos.Response<PhotoDto>.Fail("PhotoEmpty", 400));
        }
    }
}
