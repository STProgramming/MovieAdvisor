using CsvHelper;
using CsvHelper.Configuration;
using MAModels.Models;
using MAServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Net.Http.Headers;

namespace MAServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly IConfiguration _configuration;

        public FileServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<string> SaveImage(ICollection<IFormFile> Files)
        {
            List<string> pathsImage = new List<string>();
            string DirectoryUpload = Path.Combine(_configuration["ServerDirectory:Upload:root"], _configuration["ServerDirectory:Upload:image"]);
            string PathToSave = Path.Combine(Directory.GetCurrentDirectory(), DirectoryUpload);
            foreach(var file in Files)
            {
                var FileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var FullPath = Path.Combine(PathToSave, FileName);

                using (var stream = new FileStream(FullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                pathsImage.Add(FullPath);
            }
            return pathsImage;
        }

        public string MakeCsv(List<PreferenceModelTrain> model)
        {
            string nameFile = new Guid().ToString();
            var pathFile = MakePathCsv(nameFile);
            using (var writer = new StreamWriter(pathFile))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteRecords(model);
            }
            return pathFile;
        }

        private string MakePathCsv(string nameFile)
        {
            string directoryCsv = Path.Combine(_configuration["ServerDirectory:Upload:root"], _configuration["ServerDirectory:Upload:csv"]);
            string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), directoryCsv);
            return Path.Combine(pathToSave, nameFile);
        }
    }
}
