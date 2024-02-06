using MAModels.EntityFrameworkModels;

namespace MAModels.DTO
{
    public class ImageDTO : Image
    {
        public ImageDTO(
            string ImageName,
            byte[] ImageData,
            int movieId,
            Movie movie
        )
        {
            this.ImageName = ImageName;
            ImageExtension = Path.GetExtension(ImageName);
            this.ImageData = ImageData;
            MovieId = movieId;
            Movie = movie;
        }

        public ImageDTO(Image im)
        {
            ImageName = im.ImageName;
            ImageExtension = im.ImageName;
            ImageData = im.ImageData;
            MovieId = im.MovieId;
            Movie = im.Movie;
        }
    }
}
