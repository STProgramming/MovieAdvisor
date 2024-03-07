using Microsoft.ML.Data;

namespace MAModels.Models.RecommendationServices.RecommendationBasedOnReviews
{
    public class BasedOnReviewsOutput
    {
        public float Label { get; set; }

        [ColumnName("Score")]
        public float Score { get; set; }
    }
}
