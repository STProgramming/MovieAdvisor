using Microsoft.AspNetCore.Http;

namespace MAServices.Interfaces
{
    public interface IUploadFileServices
    {
        List<string> SaveImage(ICollection<IFormFile> Files);
    }
}
