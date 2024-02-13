using MAAI.Interfaces;
using MAModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace MAApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationServices _recommendationServices;

        public RecommendationController(IRecommendationServices recommendationServices)
        {
            _recommendationServices = recommendationServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string emailUser)
        {
            try
            {
                return Ok(await _recommendationServices.MoviesSuggestedByUser(emailUser));
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }
    }
}
