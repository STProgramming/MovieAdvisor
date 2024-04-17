using Microsoft.ML.Data;

namespace MAModels.Models.AI.RecommendationServices.SentimentPrediction
{
    public class SentimentPredict
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        public float Probability { get; set; }

        public float Score { get; set; }
    }
}
