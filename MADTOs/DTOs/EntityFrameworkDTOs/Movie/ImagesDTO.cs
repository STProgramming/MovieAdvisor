﻿namespace MADTOs.DTOs.EntityFrameworkDTOs.Movie
{
    public class ImagesDTO
    {
        public string ImageName { get; set; } = string.Empty;

        public string ImageExtension { get; set; } = string.Empty;

        public byte[]? ImageData { get; set; }
    }
}
