using MAContracts.Contracts.Services;
using MAContracts.Contracts.Services.movie;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MAApi.Controllers.movie
{
    [Route("api/movie/[controller]")]
    [ApiController]
    public class MultimediaController : ControllerBase
    {
        private readonly IMultimediaServices _multimediaServices;

        private readonly IFileServices _fileServices;

        public MultimediaController(IMultimediaServices multimediaServices, IFileServices fileServices)
        {
            _multimediaServices = multimediaServices;
            _fileServices = fileServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int MovieId, int Counter)
        {
            try
            {
                var image = await _multimediaServices.GetMovieImages(MovieId, Counter);
                return File(image.ImageData, $"image/{image.ImageExtension}");                
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.Conflict);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] int MovieId, List<IFormFile> Files)
        {
            try
            {
                await _multimediaServices.AddNewMovieImage(Files, MovieId, _fileServices.ConvertToByteArray(Files));
                return StatusCode((int)HttpStatusCode.Created);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }
    }
}
