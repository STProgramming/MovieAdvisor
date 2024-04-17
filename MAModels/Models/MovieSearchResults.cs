using MAModels.EntityFrameworkModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAModels.Models
{
    public class MovieSearchResults
    {
        public int MoviesCount { get; set; }

        public int ResultsCount { get; set; }

        public List<Movies> Movies { get; set; } = new List<Movies>();

        public List<Movies> ResultsForYear { get; set; } = new List<Movies>();

        public List<Movies> ResultsForLifeSpan { get; set; } = new List<Movies>();

        public List<Movies> ResultsForTitle { get; set; } = new List<Movies>();

        public List<Movies> ResultsForMaker { get; set; } = new List<Movies>();

        public List<Movies> ResultsForTag { get; set; } = new List<Movies>();

        public List<Movies> ResultsForDescription { get; set; } = new List<Movies>();
    }
}
