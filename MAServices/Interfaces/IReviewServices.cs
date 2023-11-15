﻿using MAModels.DTO;
using MAModels.EntityFrameworkModels;

namespace MAServices.Interfaces
{
    public interface IReviewServices
    {
        Task PostNewReview(User user, Movie movie, string? descriptionVote, short vote, string? when);

        Task<List<ReviewDTO>?> SearchEngineReviews(User? user, Movie? movie);
    }
}
