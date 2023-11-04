﻿using MAModels.EntityFrameworkModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MAModels.DTO
{
    public class MovieDTO : Movie
    {
        public List<int>? MovieTagsId { get; set; }

        public string? FileName { get; set; }


        public MovieDTO(string MovieTitle, short MovieYearProduction, string MovieDescription, string MovieMaker, bool IsForAdult) : base() 
        {
            this.MovieTitle = MovieTitle;
            this.MovieYearProduction = MovieYearProduction;
            this.MovieDescription = MovieDescription;
            this.MovieMaker = MovieMaker;
            this.IsForAdult = IsForAdult;
        }

        public MovieDTO() { }

        public void InsertMovieTagsId()
        {
            if (MovieTagsId != null && MovieTagsId.Count > 0)
            {
                this.MovieTagsList = new List<MovieDescription>();
                foreach (var item in MovieTagsId)
                {
                    var tag = new MovieDescription
                    {
                        MovieId = this.MovieId,
                        MovieTagId = item
                    };
                    this.MovieTagsList.Add(tag);
                }
            }
        }

        public void InsertPhoto()
        {
            if (!string.IsNullOrEmpty(this.FileName))
            {                
                string pathDir = @"D:\\STCorp-cartella utenti\\Immagini\\Foto progetto";
                string path = Path.Combine(pathDir, this.FileName);
                this.MovieImage = System.IO.File.ReadAllBytes(path);
            }
        }
    }
}
