﻿using MAModels.EntityFrameworkModels;
using MAServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MAServices.Services
{
    public class ReviewServices : IReviewServices
    {
        private readonly ApplicationDbContext _database;

        public ReviewServices(ApplicationDbContext database)
        {
            _database = database;
        }

        public async Task PostNewReview(User user, Movie movie, string? descriptionVote, int vote)
        {
            Review newReview = new Review 
            { 
                UserId = user.UserId, 
                MovieId = movie.MovieId, 
                Movie = movie,
                User = user, 
                Vote = vote, 
                DescriptionVote = descriptionVote 
            };
            
            await _database.Reviews.AddAsync(newReview);
            await _database.SaveChangesAsync();            
        }

        public async Task<ICollection<Review>> GetReviews()
        {
            return await _database.Reviews.ToListAsync();
        }

        public async Task<ICollection<Review>> GetReviewsOfUser(int userId)
        {
            return await _database.Reviews.Where(r => r.UserId == userId).ToListAsync();
        }

        public async Task<ICollection<Review>> GetReviewsOfMovie(int movieId)
        {
            return await _database.Reviews.Where(r => r.MovieId == movieId).ToListAsync();
        }

        public async Task<Review> GetYourRiviewOfMovie(int userId, int movieId)
        {
            return await _database.Reviews.Where(r => r.MovieId == movieId && r.UserId == userId).FirstOrDefaultAsync();
        }
    }
}
