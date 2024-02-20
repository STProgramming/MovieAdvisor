using MAServices.Interfaces;
using MAServices.Interfaces.movie;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Get(int movieId)
        {
            try
            {
                var image = await _multimediaServices.GetMovieImages(movieId);
                return File(image.ImageData, $"image/{image.ImageExtension}");                
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] int MovieId, List<IFormFile> Files)
        {
            try
            {
                await _multimediaServices.AddNewMovieImage(Files, MovieId, _fileServices.ConvertToByteArray(Files));
                return StatusCode(201);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }
    }
}
