using Microsoft.ML.Data;

namespace MAModels.Models
{
    public class ModelInput
    {
        [LoadColumn(0)]
        public float UserId { get; set; }

        [LoadColumn(1)]
        public float MovieId { get; set; }

        [LoadColumn(2)]
        public string MovieTitle { get; set; } = string.Empty;

        [LoadColumn(3)]
        public string MovieDescription { get; set;} = string.Empty;

        [LoadColumn(4)]
        public string MovieMaker { get; set; } = string.Empty;

        [LoadColumn(5)]
        public string UserName { get; set; } = string.Empty;

        [LoadColumn(6)]
        public string MovieGenres {  get; set; } = string.Empty;

        [LoadColumn(7)]
        public string ReviewDate {  get; set; } = string.Empty;

        [LoadColumn(8)]
        public float Label { get; set; }
    }
}
