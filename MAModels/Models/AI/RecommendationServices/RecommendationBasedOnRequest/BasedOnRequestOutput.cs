using Microsoft.ML.Data;

namespace MAModels.Models.AI.RecommendationServices.RecommendationBasedOnRequest
{
    public class BasedOnRequestOutput
    {
        public float Label { get; set; }

        [ColumnName("Score")]
        public float Score { get; set; }
    }
}
