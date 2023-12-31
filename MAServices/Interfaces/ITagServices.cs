﻿using MAModels.DTO;
using MAModels.EntityFrameworkModels;

namespace MAServices.Interfaces
{
    public interface ITagServices
    {
        Task<Tag?> GetTag(int tagId);

        Task<List<Tag>> GetAllTags();

        Task<List<MovieDTO>> GetMoviesFromTag(int tagId);

        Task CreateAllTags();
    }
}
