using MAModels.Models;
using Microsoft.AspNetCore.Http;

namespace MAServices.Interfaces
{
    public interface IFileServices
    {
        List<string> SaveImage(ICollection<IFormFile> Files);

        string MakeCsv(List<PreferenceModelTrain> model);
    }
}
