﻿using MAContracts.Contracts.Services;
using MAContracts.Contracts.Services.Movie;
using MAModels.Enumerables.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MAApi.Controllers.Movie
{
    [Route("api/Movie/[controller]")]
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

        [AllowAnonymous]
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
                //image is a not necessary field right now
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.Conflict);
            }
        }

        [Authorize(Roles = nameof(ERoleUser.AppAdmin))]
        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] int MovieId, [FromForm] List<IFormFile> Files)
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
