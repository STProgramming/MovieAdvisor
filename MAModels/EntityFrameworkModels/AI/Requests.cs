using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAModels.EntityFrameworkModels.AI
{
    [Table("Requests")]
    public class Requests
    {
        [Key]
        public int RequestId { get; set; }

        public string WhatClientWants { get; set; } = string.Empty;

        public string HowClientFeels { get; set; } = string.Empty;

        public bool? Sentiment { get; set; }

        public DateTime DateTimeRequest { get; set; }

        [Required]
        public int SessionId { get; set; }

        public Sessions Session { get; set; } = new Sessions();

        public virtual List<Recommendations> RecommendationsList { get; set; } = new List<Recommendations>();

        public Requests() { }
    }
}
