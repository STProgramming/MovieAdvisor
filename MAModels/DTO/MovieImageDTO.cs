using MAModels.EntityFrameworkModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAModels.DTO
{
    public class MovieImageDTO : MovieImage
    {
        public MovieImageDTO(
            string ImageName,
            string pathServer,
            int movieId,
            Movie movie
        )
        {
            MovieImageName = ImageName;
            MovieImageExtension = Path.GetExtension(MovieImageName);
            MovieImagePath = Path.Combine(pathServer, MovieImageName);
            MovieId = movieId;
            Movie = movie;
        }

        public MovieImageDTO(MovieImage im)
        {
            MovieImageName = im.MovieImageName;
            MovieImageExtension = im.MovieImageName;
            MovieImagePath = im.MovieImagePath;
            MovieId = im.MovieId;
            Movie = im.Movie;
        }

    }
}
