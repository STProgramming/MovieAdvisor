using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAModels.Models
{
    public class PreferenceModelTrain
    {
        [LoadColumn(0)]
        public int UserId { get; set; }

        [LoadColumn(1)]
        public int MovieId { get; set; }

        [LoadColumn(2)]
        public float Label { get; set; }
    }
}
