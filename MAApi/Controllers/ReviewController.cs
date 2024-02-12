using MAModels.EntityFrameworkModels;
using MAServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MAApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewServices _reviewServices;

        public ReviewController(IReviewServices reviewServices, IUserServices userServices, IMovieServices movieServices) 
        {
            _reviewServices = reviewServices;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string? emailUser, int? movieId)
        {
            try
            {
                return Ok(await _reviewServices.SearchEngineReviews(emailUser, movieId));
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(string emailUser, int movieId, string? descriptionVote, float vote, string? when)
        {
            try
            {
                await _reviewServices.PostNewReview(emailUser, movieId, descriptionVote, vote, when);
                return StatusCode(201);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }        
    }
}
