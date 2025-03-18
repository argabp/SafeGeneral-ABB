using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace ABB.Application.Users.Commands
{
    public class FileValidatorCondition
    {
        public bool OnlyImageExtension(IFormFile ProfilePhoto)
        {
            var contentType = ProfilePhoto.ContentType.ToLower();
            return contentType == "image/jpeg"
                   || contentType == "image/jpg"
                   || contentType == "image/png";
        }
    }
}