using MAModels.Models;
using Microsoft.AspNetCore.Http;

namespace MAServices.Interfaces
{
    public interface IFileServices
    {
        List<byte[]> ConvertToByteArray(ICollection<IFormFile> Files);
    }
}
