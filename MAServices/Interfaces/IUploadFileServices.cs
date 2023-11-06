using Microsoft.AspNetCore.Http;

namespace MAServices.Interfaces
{
    public interface IUploadFileServices
    {
        void SaveImage(IFormFileCollection Files);
    }
}
