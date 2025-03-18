using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ABB.Application.Common.Helpers
{
    public interface IProfilePictureHelper
    {
        string GetProfilePicture(string photo);
        Task<string> Upload(IFormFile image);
        Task<string> UploadToFolder(IFormFile image, string path);
        void UploadRawFile(string file, string fileName, string path);
        void UploadByteFile(byte[] file, string fileName, string path);
    }
}