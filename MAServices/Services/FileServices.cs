using MAContracts.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MAServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly IConfiguration _configuration;

        public FileServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<byte[]> ConvertToByteArray(ICollection<IFormFile> Files)
        {
            List<byte[]> resultImages = new List<byte[]>();
            foreach (var item in Files)
            {
                byte[] img = ByteArrayFromPathImage(Path.Combine(_configuration["UploadFilePaths:ImagePath"], item.FileName));
                resultImages.Add(img);
            }
            return resultImages;
        }

        private byte[] ByteArrayFromPathImage(string pathImage)
        {
            if (!File.Exists(pathImage)) throw new FileNotFoundException();

            // Leggi il contenuto del file in un array di byte.
            byte[] imageData;
            using (FileStream fileStream = new FileStream(pathImage, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    imageData = binaryReader.ReadBytes((int)fileStream.Length);
                }
            }
            return imageData;
        }
    }
}
