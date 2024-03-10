using MAContracts.Contracts.Services;
using MAContracts.Contracts.Services.Identity.User;
using MADTOs.DTOs.ModelsDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Get(int? MovieId)
        {
            try
            {
                return Ok(await _reviewServices.SearchEngineReviews(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, MovieId));
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
                return StatusCode(201);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(406);
            }
        }        
    }
}
