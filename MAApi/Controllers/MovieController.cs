using MAContracts.Contracts.Services;
using MADTOs.DTOs.EntityFrameworkDTOs;
using MADTOs.DTOs.ModelsDTOs;
using MAModels.Enumerables.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MAApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieServices _movieServices;

        public MovieController(IMovieServices movieServices)
        {
            _movieServices = movieServices;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(string Search = "", short Page = 1, short Elements = 9)
        {
            return Ok(await _movieServices.SearchEngine(Search, Page, Elements));
        }

        [Authorize(Roles = nameof(ERoleUser.AppAdmin))]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]NewMovieDTO NewMovie)
        {
            try
            {               
                return StatusCode(201, await _movieServices.CreateNewMovie(NewMovie));
            }
            catch (IOException)
            {
                return StatusCode((int)HttpStatusCode.Conflict);
            }
        }
    }
}
