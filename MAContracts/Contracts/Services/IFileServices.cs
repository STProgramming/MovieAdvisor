using Microsoft.AspNetCore.Http;

namespace MAContracts.Contracts.Services
{
    public interface IFileServices
    {
        List<byte[]> ConvertToByteArray(ICollection<IFormFile> Files);
    }
}
