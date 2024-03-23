namespace MADTOs.DTOs.EntityFrameworkDTOs.AI
{
    public class RequestsDTO
    {
        public int RequestId { get; set; }

        public string WhatClientWants { get; set; } = string.Empty;
        
        public string HowClientFeels { get; set; } = string.Empty;

        public bool? Sentiment {  get; set; }

        public List<RecommendationsDTO> Recommendations { get; set; } = new List<RecommendationsDTO>();

        public DateTime DateTimeRequest { get; set; }
    }
}
