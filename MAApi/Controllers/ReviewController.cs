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

        private readonly EmailAddressAttribute _emailController = new EmailAddressAttribute();

        private readonly IUserServices _userServices;

        private readonly IMovieServices _movieServices;

        public ReviewController(IReviewServices reviewServices, IUserServices userServices, IMovieServices movieServices) 
        {
            _reviewServices = reviewServices;
            _userServices = userServices;
            _movieServices = movieServices;
        }

        [HttpPost]
        public async Task<IActionResult> PostNewReview(string emailUser, int movieId, string? descriptionVote, float vote, string? when)
        {
            if (string.IsNullOrEmpty(emailUser) || !_emailController.IsValid(emailUser)) return StatusCode(406);
            var user = await _userServices.GetUserFromEmail(emailUser);
            if (user == null) return StatusCode(401);
            var movie = await _movieServices.GetMovieData(movieId);
            if (movie == null) return NotFound();
            await _reviewServices.PostNewReview(user, movie, descriptionVote, vote, when);
            return StatusCode(201);
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews(string? emailUser, int? movieId)
        {
            User? user = await _userServices.GetUserFromEmail(emailUser);
            Movie? movie = await _movieServices.GetMovieData(Convert.ToInt32(movieId));
            var results = await _reviewServices.SearchEngineReviews(user, movie);
            return results != null && results.Count > 0 ? Ok(results) : NotFound();
        }
    }
}
