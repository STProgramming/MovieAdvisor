using MAContracts.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Get(string? EmailUser, int? MovieId)
        {
            try
            {
                return Ok(await _reviewServices.SearchEngineReviews(EmailUser, MovieId));
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(string EmailUser, int MovieId, string? DescriptionVote, float Vote, string? When)
        {
            try
            {
                await _reviewServices.PostNewReview(EmailUser, MovieId, DescriptionVote, Vote, When);
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
