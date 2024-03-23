using MAContracts.Contracts.Services;
using MAContracts.Contracts.Services.Identity.User;
using MADTOs.DTOs.ModelsDTOs;
using MAModels.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace MAApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewServices _reviewServices;

        public ReviewController(IReviewServices reviewServices, IUserServices userServices, IMovieServices movieServices) 
        {
            _reviewServices = reviewServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string? Search)
        {
            try
            {
                return Ok(await _reviewServices.SearchEngineReviews(null, Search));
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpGet("GetUserReviews")]
        public async Task<IActionResult> GetUserReviews(string? Search)
        {
            try
            {
                return Ok(await _reviewServices.SearchEngineReviews(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, Search));
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]NewReviewDTO newReview)
        {
            try
            {
                await _reviewServices.PostNewReview(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, newReview);
                return StatusCode((int)HttpStatusCode.Created);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
            catch (ConflictException)
            {
                return StatusCode((int)HttpStatusCode.Conflict);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.NotAcceptable);
            }            
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Put([FromQuery]int reviewId, [FromBody] NewReviewDTO newReview)
        {
            try
            {
                await _reviewServices.EditReview(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, reviewId ,newReview);
                return StatusCode((int)HttpStatusCode.Created);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.NotAcceptable);
            }
        }
    }
}
