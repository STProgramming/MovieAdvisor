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
        public async Task<IActionResult> PostNewReview(string emailUser, int movieId, string? descriptionVote, short vote)
        {
            if (string.IsNullOrEmpty(emailUser) || !_emailController.IsValid(emailUser)) return StatusCode(406);
            var user = await _userServices.GetUserData(emailUser);
            if (user == null) return StatusCode(401);
            var movie = await _movieServices.GetMovieData(movieId);
            if (movie == null) return NotFound();
            await _reviewServices.PostNewReview(user, movie, descriptionVote, vote);
            return StatusCode(201);
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews(string? emailUser, int? movieId)
        {
            if (string.IsNullOrEmpty(emailUser) && movieId == null) return Ok(await _reviewServices.GetReviews());
            else if (string.IsNullOrEmpty(emailUser) && movieId != null && movieId > 0) return Ok(await _reviewServices.GetReviewsOfMovie(Convert.ToInt32(movieId)));
            else if (!string.IsNullOrEmpty(emailUser) && _emailController.IsValid(emailUser) && movieId == null)
            {
                var user = await _userServices.GetUserData(emailUser);
                if (user != null) return Ok(await _reviewServices.GetReviewsOfUser(user.UserId));
            }
            else
            {
                var user = await _userServices.GetUserData(emailUser);
                if (user != null) return Ok(await _reviewServices.GetYourRiviewOfMovie(user.UserId, Convert.ToInt32(movieId)));
            }
            return BadRequest();
        }
    }
}
