using Microsoft.ML.Data;

namespace MAModels.Models.RecommendationServices.RecommendationBasedOnSentiments
{
    public class BasedOnRequestOutput
    {
        public float Label { get; set; }

        [ColumnName("Score")]
        public float Score { get; set; }
    }
}
