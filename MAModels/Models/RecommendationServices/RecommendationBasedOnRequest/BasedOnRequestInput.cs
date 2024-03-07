using Microsoft.ML.Data;

namespace MAModels.Models.RecommendationServices.RecommendationBasedOnSentiments
{
    public class BasedOnRequestInput
    {
        [LoadColumn(0)]
        public string UserId { get; set; }

        [LoadColumn(1)]
        public float MovieId { get; set; }

        [LoadColumn(2)]
        public string MovieTitle { get; set; } = string.Empty;

        [LoadColumn(3)]
        public string WhatClientWants { get; set; } = string.Empty;

        [LoadColumn(4)]
        public string HowClientFeels { get; set; } = string.Empty;

        [LoadColumn(5)]
        public float Label1 { get; set; }

        [LoadColumn(6)]
        public bool Label2 { get; set; }
    }
}
