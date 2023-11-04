using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAServices.Interfaces
{
    public interface IMovieTagServices
    {
        Task<MovieTagDTO> GetMovieTag(int movieTagId);

        Task SetListMovieTag(List<MovieDescription> movieDescList);
    }
}
