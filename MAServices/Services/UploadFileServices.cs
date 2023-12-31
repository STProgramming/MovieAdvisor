﻿using MAServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace MAServices.Services
{
    public class UploadFileServices : IUploadFileServices
    {
        private readonly IConfiguration _configuration;

        public UploadFileServices(IConfiguration configuration)
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
    }
}
