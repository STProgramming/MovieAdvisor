using MAContracts.Contracts.Services.AI;
using MADTOs.DTOs.ModelsDTOs.AI;
using MAModels.Exceptions.AI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace MAApi.Controllers.AI
{
    [Authorize]
    [Route("api/AI/[controller]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationServices _recommendationServices;

        public RecommendationController(IRecommendationServices recommendationServices)
        {
            _recommendationServices = recommendationServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _recommendationServices.RecommendationsBasedOnReviews(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value));
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
            catch (InsufficientReviewsException)
            {
                return StatusCode((int)HttpStatusCode.UnprocessableEntity);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]NewRequestDTO requestUser)
        {
            //try
            //{
            //    return Ok(await _recommendationServices.RecommendationsBasedOnRequest(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, requestUser));
            //}
            //catch (NullReferenceException)
            //{
            //    return NotFound();
            //}
            throw new NotImplementedException();
        }
    }
}
