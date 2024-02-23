using MAContracts.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace MAApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationServices _recommendationServices;

        public RecommendationController(IRecommendationServices recommendationServices)
        {
            _recommendationServices = recommendationServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string EmailUser)
        {
            try
            {
                return Ok(await _recommendationServices.MoviesSuggestedByUser(EmailUser));
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }
    }
}
