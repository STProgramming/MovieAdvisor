using MAModels.EntityFrameworkModels;

namespace MAModels.DTO
{
    public class ImageDTO : Image
    {
        public ImageDTO(
            string ImageName,
            string pathServer,
            int movieId,
            Movie movie
        )
        {
            this.ImageName = ImageName;
            ImageExtension = Path.GetExtension(ImageName);
            ImagePath = Path.Combine(pathServer, ImageName);
            MovieId = movieId;
            Movie = movie;
        }

        public ImageDTO(Image im)
        {
            ImageName = im.ImageName;
            ImageExtension = im.ImageName;
            ImagePath = im.ImagePath;
            MovieId = im.MovieId;
            Movie = im.Movie;
        }
    }
}
