using Microsoft.ML.Data;

namespace MAModels.Models
{
    public class ModelOutput
    {
        public float Label { get; set; }

        [ColumnName("Score")]
        public float Score { get; set; }
    }
}
