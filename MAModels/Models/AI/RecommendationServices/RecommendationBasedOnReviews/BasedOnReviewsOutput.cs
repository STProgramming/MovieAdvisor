using Microsoft.ML.Data;

namespace MAModels.Models.AI.RecommendationServices.RecommendationBasedOnReviews
{
    public class BasedOnReviewsOutput
    {
        public float Label { get; set; }

        [ColumnName("Score")]
        public float Score { get; set; }
    }
}
