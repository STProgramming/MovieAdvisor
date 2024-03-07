using Microsoft.ML.Data;

namespace MAModels.Models.RecommendationServices.SentimentPrediction
{
    public class SentimentIssue
    {
        [LoadColumn(0)]
        public bool Label { get; set; }

        [LoadColumn(1)]
        public string HowClientFeels { get; set; }
    }
}
